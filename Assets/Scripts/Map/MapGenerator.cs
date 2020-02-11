using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Noise;
using Random = System.Random;
using TweenTypes = Interp.TweenTypes;

namespace Map.Generation
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Generator Settings")]
        public bool autoUpdate;

        [Header("Terrain Settings")]
        public int mapSize = 100;

        public int mapChunks;

        public int resPower = 5;

        public float terrainHeight;

        public float seaLevel;

        [Header("Noise Settings")]
        public NoiseType noiseType;

        public int seed;

        public float2 terrainOffset;

        public float frequency = 1;

        public int divisions = 0;

        [Header("Fractal Settings")]
        public FractalType fractalType;

        public int octaves = 1;

        public float persistance = .5f;

        public float lacunarity = 2;

        public void Update()
        {
            var filtersGen = Enumerable
                .Range(0, octaves)
                .Select(x => noiseBuilder(noiseType, getSeed(x)))
                .ToArray();


            Generator fractalGen = null;
            switch (fractalType)
            {
                case FractalType.None:
                    fractalGen = filtersGen[0];
                    break;

                case FractalType.FBM:
                    fractalGen = new FBM(lacunarity, persistance, filtersGen);
                    break;

                case FractalType.Multifractal:
                    fractalGen = new Multifractal(lacunarity, persistance, filtersGen);
                    break;

                case FractalType.SlopeErosion:
                    fractalGen = new SlopeErosion(lacunarity, persistance, filtersGen);
                    break;
            }

            fractalGen = fractalGen.Warp(4, 1);

            Generator seaGen;
            if (seaLevel > 0)
            {
                seaGen = new Max(fractalGen, new Constant(seaLevel));
            }
            else
            {
                seaGen = fractalGen;
            }


            Generator finalGen = seaGen;

            GetComponent<MapRenderer>().DrawTerrain(new float2(transform.position.x, transform.position.z), float2.zero, finalGen, terrainHeight, resPower, mapSize, mapChunks, frequency);
        }

        private int getSeed(int x)
            => (int)(seed + new Random(seed * x).NextDouble());

        private Generator noiseBuilder(NoiseType type, int seed)
        {
            switch (type)
            {
                case NoiseType.White:
                    return new WhiteNoise(seed);

                case NoiseType.Sin:
                    return new SinNoise();

                case NoiseType.Square:
                    return new SquareNoise();

                case NoiseType.Triangle:
                    return new TriangleNoise();

                case NoiseType.Sawtooth:
                    return new SawtoothNoise();

                case NoiseType.Value:
                    return new Interpolate(new WhiteNoise(seed), TweenTypes.SmootherStep);

                case NoiseType.Perlin:
                    return new PerlinNoise(seed);

                default:
                    return null;
            }

            // return new Warp(gen,
            //     new Interpolate(new WhiteNoise(seed + 10), TweenTypes.SmootherStep),
            //     new Interpolate(new WhiteNoise(seed + 2), TweenTypes.SmootherStep),
            //     new Interpolate(new WhiteNoise(seed + 39), TweenTypes.SmootherStep), , 1);

            // return new Interpolate(new MidpointDisplacement(gen, new WhiteNoise(seed + 1), math.pow(2, divisions), divisions));

            // return Mathg.Lerp(
            //     new PerlinNoise(seed + 53).Filter(FilterType.Billow),
            //     new PerlinNoise(seed + 71).Filter(FilterType.Ridged),
            //     new PerlinNoise(seed).Scale(2)
            // );

            // return new PerlinNoise(seed).Scale(2);
        }

        public enum NoiseType
        {
            White,
            Sin,
            Square,
            Triangle,
            Sawtooth,
            Value,
            Perlin
        }

        public enum FilterGenType
        {
            None,
            Norm,
            Billow,
            Ridged,
            Interpolate,
            MidpointDisplacement
        }

        public enum FractalType
        {
            None,
            FBM,
            Multifractal,
            SlopeErosion
        }
    }
}