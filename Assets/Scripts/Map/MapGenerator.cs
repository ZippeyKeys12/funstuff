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

        [Header("Filter Settings")]
        public int filterCount;

        public Dictionary<int, FilterType> filterType = new Dictionary<int, FilterType>();

        public Dictionary<int, float> filterUnit = new Dictionary<int, float>();

        public Dictionary<int, NoiseType> secondaryNoiseType = new Dictionary<int, NoiseType>();
        public Dictionary<int, int> secondarySeed = new Dictionary<int, int>();
        public Dictionary<int, float> secondaryFreq = new Dictionary<int, float>();

        public Dictionary<int, int> windowSize = new Dictionary<int, int>();
        public Dictionary<int, int> divisions = new Dictionary<int, int>();

        [Header("Fractal Settings")]
        public FractalType fractalType;

        public int octaves = 1;

        public float persistance = .5f;

        public float lacunarity = 2;

        public void GenerateMap()
        {
            Func<int, int> getSeed = x => (int)(seed + new Random(seed * x).NextDouble());


            Func<NoiseType, int, Generator> noiseBuilder = (type, seed) =>
            {
                switch (type)
                {
                    case NoiseType.White:
                        return new WhiteNoise(seed);

                    case NoiseType.Sin:
                        return new SinNoise(seed);

                    case NoiseType.Value:
                        return new Interpolate(new WhiteNoise(seed), TweenTypes.SmootherStep);

                    case NoiseType.Perlin:
                        return new PerlinNoise(seed);

                    default:
                        return null;
                }
            };

            Func<Generator, Generator> filter = x => x;
            for (var i = 0; i < filterCount; i++)
            {
                FilterDefaults(i);

                switch (filterType[i])
                {
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
                        var unit = filterUnit[i];
                        filter = x => new Interpolate(x, unit);
                        break;

                    case FilterType.MidpointDisplacement:
                        var noise = noiseBuilder(secondaryNoiseType[i], secondarySeed[i]);
                        var wSize = windowSize[i];
                        var divs = divisions[i];
                        filter = x => new MidpointDisplacement(x, noise, wSize, divs);
                        break;
                }
            }


            var generators = Enumerable.Range(0, octaves).Select(x => filter(noiseBuilder(noiseType, getSeed(x)))).ToArray();


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

            GetComponent<MapRenderer>().DrawTerrain(new float2(transform.position.x, transform.position.z), terrainOffset, finalGen, terrainHeight, resPower, mapSize, mapChunks, frequency);
        }

        public void FilterDefaults(int index)
        {
            if (!filterType.ContainsKey(index))
            {
                filterType[index] = FilterType.None;
                filterUnit[index] = 1;
                secondaryNoiseType[index] = NoiseType.White;
                secondarySeed[index] = 0;
                secondaryFreq[index] = 1;
                windowSize[index] = 1;
                divisions[index] = 0;
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
            Interpolate,
            MidpointDisplacement
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