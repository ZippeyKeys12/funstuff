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
        [Header("Render Settings")]
        public int mapSize = 100;

        [Range(0, 4)]
        public int mapChunks;

        [Range(5, 8)]
        public int resolutionPower = 5;

        [Range(0, 500)]
        public float terrainHeight;

        [Range(0, 1)]
        public float seaLevel;

        [Header("Noise Settings")]
        [Range(0, 10000)]
        public int seed;

        [Range(0, 2500)]
        public float offsetX, offsetY;

        public NoiseType noiseType;

        [Range(.01f, 1f)]
        public float frequency = .5f;

        [Header("Filter Settings")]
        public FilterType filterType;

        [Header("Fractal Settings")]
        public FractalType fractalType;

        [Range(1, 10)]
        public int octaves = 1;

        [Range(.0001f, 1f)]
        public float persistance = .5f;

        [Range(.0001f, 5)]
        public float lacunarity = 2;

        public bool autoUpdate;

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
                    filter = x => new Interpolate(x);
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

            GetComponent<MapRenderer>().DrawTerrain(new float2(transform.position.x, transform.position.z), new float2(-offsetY, offsetX), finalGen, terrainHeight, resolutionPower, mapSize, mapChunks, frequency);
        }
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