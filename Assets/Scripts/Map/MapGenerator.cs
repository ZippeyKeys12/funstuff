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
                    noiseBuilder = x => new ValueNoise(getSeed(x));
                    break;

                case NoiseType.Perlin:
                    noiseBuilder = x => new PerlinNoise(getSeed(x));
                    break;

                case NoiseType.Billow:
                    noiseBuilder = x => new Billow(new PerlinNoise(getSeed(x)));
                    break;

                case NoiseType.Ridged:
                    noiseBuilder = x => new Ridged(new PerlinNoise(getSeed(x)));
                    break;

                case NoiseType.Value2:
                    noiseBuilder = x => new Interpolate(new WhiteNoise(getSeed(x)), TweenTypes.SmootherStep);
                    break;
            }

            var noise = Enumerable.Range(0, octaves).Select(noiseBuilder).ToArray();

            Generator gen = null;
            switch (fractalType)
            {
                case FractalType.FBM:
                    gen = new FBM(lacunarity, persistance, noise);
                    break;

                case FractalType.Multifractal:
                    gen = new Multifractal(lacunarity, persistance, noise);
                    break;

                case FractalType.Norm:
                    gen = new Norm(noise);
                    break;
            }

            // var g1 = new ValueNoise(seed);
            // var g2 = new Interpolate(new WhiteNoise(seed));
            // for (var x = 0f; x < 100; x += .5f)
            // {
            //     var v = (float)new Random().NextDouble();
            //     var val1 = g1.Get(new float3(x/*, v, v + 1*/), frequency);
            //     var val2 = g2.Get(new float3(x/*, v, v + 1*/), frequency);
            //     var nan = math.isnan(val1.Gradient) | math.isnan(val2.Gradient);
            //     if (val1 != val2 && !nan.x && !nan.y)
            //     {
            //         print($"({x}, {v}, {v + 1}) - {val1.Gradient} != {val2.Gradient}");
            //         break;
            //     }
            // }

            if (seaLevel > 0)
            {
                gen = new Max(gen, new Constant(seaLevel));
            }

            GetComponent<MapRenderer>().DrawTerrain(new float2(transform.position.x, transform.position.z), new float2(offsetY, offsetX) + 100, gen, terrainHeight, resolutionPower, mapSize, mapChunks, frequency);
        }
    }

    public enum NoiseType
    {
        White,
        Sin,
        Value,
        Value2,
        Perlin,
        Billow,
        Ridged
    }

    public enum FractalType
    {
        FBM,
        Multifractal,
        Norm
    }
}