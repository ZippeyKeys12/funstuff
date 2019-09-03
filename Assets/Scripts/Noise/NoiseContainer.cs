using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class NoiseContainer {
    public NoiseContainer() {
        UpdateInst();
    }
    
    public INoise inst;

    public NoiseOperatorType type = NoiseOperatorType.Generative;

    #region GenerativeTypes
    public GenerativeNoiseType genType = GenerativeNoiseType.Random;

    [Min(1)]
    public int mapWidth = 100, mapHeight = 100;

    public int seed;

    [Range(0, 200)]
    public int nb;
    #endregion

    #region PolyTypes
    public PolyNoiseType polyType;

    [Range(0, 1)]
    public float persistance, lacunarity;
    
    public NoiseContainer[] polyOperands;
    #endregion


    [OnSerializing]
    private void OnSerializing(StreamingContext ctx) {

    }

    [OnDeserialized]
    private void OnDeserialized(object obj) {

    }

    public void UpdateInst() {
        switch (type) {
            case NoiseOperatorType.Generative:
                switch (genType) {
                    case GenerativeNoiseType.Random:
                        inst = new RandomNoise() {
                            w = mapWidth,
                            h = mapHeight,
                            seed = seed
                        };
                        break;
                    case GenerativeNoiseType.Value:
                        inst = new ValueNoise() {
                            w = mapWidth,
                            h = mapHeight,
                            seed = seed
                        };
                        break;
                    case GenerativeNoiseType.InverseDistance:
                        inst = new InvDistNoise() {
                            w = mapWidth,
                            h = mapHeight,
                            seed = seed,
                            nb = nb
                        };
                        break;
                    case GenerativeNoiseType.Perlin:
                        inst = new PerlinNoise() {
                            w = mapWidth,
                            h = mapHeight,
                            seed = seed
                        };
                        break;
                }
                break;
            case NoiseOperatorType.Unary:
                break;
            case NoiseOperatorType.Binary:
                break;
            case NoiseOperatorType.Poly:
                foreach (var op in polyOperands) {
                    //op.SetCommon(mapWidth, mapHeight, seed);
                    op.UpdateInst();
                }

                switch (polyType) {
                    case PolyNoiseType.Average:
                        inst = new AverageFilter() {
                            w = mapWidth,
                            h = mapWidth,
                            seed = seed,
                            operands = polyOperands
                        };
                        break;
                    case PolyNoiseType.Octave:
                        inst = new OctaveFilter {
                            w = mapWidth,
                            h = mapHeight,
                            seed = seed,
                            lacunarity = lacunarity,
                            persistance = persistance,
                            operands = polyOperands
                        };
                        break;
                }
                break;
        }
    }

    public void SetCommon(int w, int h, int seed) {
        mapWidth = w;
        mapHeight = h;
        this.seed = seed;

        switch (type) {
            case NoiseOperatorType.Poly:
                foreach (var op in polyOperands) {
                    op.SetCommon(w, h, seed);
                }
                break;
        }
    }

    public void GenerateNoise() {
        inst.GenerateNoise();
    }

    public float[,] GetNoise(float scale) {
        return inst.GetNoise(scale);
    }

    public float AtXY(float x, float y) {
        return inst.AtXY(x, y);
    }
}

public enum NoiseOperatorType {
    Generative,
    Unary,
    Binary,
    Poly
}

public enum GenerativeNoiseType {
    Random,
    Value,
    InverseDistance,
    BellShaped,
    Perlin
}

public enum PolyNoiseType {
    Average,
    Octave
}
