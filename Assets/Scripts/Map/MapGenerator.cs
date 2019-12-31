using System;
using System.Linq;
using System.Reflection;
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

        public float offsetX, offsetY;

        public float frequency = .5f;

        [Header("Filter Settings")]
        public FilterType filterType;

        public float interpolationPower;

        [Header("Fractal Settings")]
        public FractalType fractalType;

        public int octaves = 1;

        public float persistance = .5f;

        public float lacunarity = 2;

        public void GenerateMap()
        {
            Func<int, int> getSeed = x => (int)(seed + new Random(seed * x).NextDouble());


            Func<int, Generator> noiseBuilder = null;
            switch (noiseType)
            {
                case NoiseType.White:
                    noiseBuilder = x => new WhiteNoise(getSeed(x));
                    break;

                case NoiseType.Sin:
                    noiseBuilder = x => new SinNoise(getSeed(x));
                    break;

                case NoiseType.Value:
                    noiseBuilder = x => new Interpolate(new WhiteNoise(getSeed(x)), TweenTypes.SmootherStep);
                    break;

                case NoiseType.Perlin:
                    noiseBuilder = x => new PerlinNoise(getSeed(x));
                    break;
            }


            Func<Generator, Generator> filter = null;
            switch (filterType)
            {
                case FilterType.None:
                    filter = x => x;
                    break;

                case FilterType.Norm:
                    filter = x => new Norm(x);
                    break;

                case FilterType.Billow:
                    filter = x => new Billow(x);
                    break;

                case FilterType.Ridged:
                    filter = x => new Ridged(x);
                    break;

                case FilterType.Interpolate:
                    filter = x => new Interpolate(x, interpolationPower);
                    break;
            }


            var generators = Enumerable.Range(0, octaves).Select(x => filter(noiseBuilder(x))).ToArray();


            Generator fractalGen = null;
            switch (fractalType)
            {
                case FractalType.None:
                    fractalGen = generators[0];
                    break;

                case FractalType.FBM:
                    fractalGen = new FBM(lacunarity, persistance, generators);
                    break;

                case FractalType.Multifractal:
                    fractalGen = new Multifractal(lacunarity, persistance, generators);
                    break;

                case FractalType.Norm:
                    fractalGen = new Norm(generators[0]);
                    break;
            }


            Generator seaGen = null;
            if (seaLevel > 0)
            {
                seaGen = new Max(fractalGen, new Constant(seaLevel));
            }
            else
            {
                seaGen = fractalGen;
            }


            var finalGen = seaGen;

            GetComponent<MapRenderer>().DrawTerrain(new float2(transform.position.x, transform.position.z), new float2(-offsetY, offsetX), finalGen, terrainHeight, resPower, mapSize, mapChunks, frequency);
        }

        public enum NoiseType
        {
            White,
            Sin,
            Value,
            Perlin
        }

        public enum FilterType
        {
            None,
            Norm,
            Billow,
            Ridged,
            Interpolate
        }

        public enum FractalType
        {
            None,
            FBM,
            Multifractal,
            Norm
        }
    }
}