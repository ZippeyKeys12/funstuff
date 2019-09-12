//using System;
//using System.Runtime.Serialization;
//using UnityEngine;

//[Serializable]
//public class NoiseContainer
//{
//    public NoiseContainer()
//    {
//        UpdateInst();
//    }

//    public INoiseModule inst;

//    public bool isChild;

//    public NoiseOperatorType type = NoiseOperatorType.Generative;

//    #region GenerativeTypes
//    public GenerativeNoiseType genType = GenerativeNoiseType.Value;

//    [Range(0, 1000000)]
//    public int seed;

//    [Range(1, 200)]
//    public int nb = 1;

//    [Range(1, 100)]
//    public int influence = 30;

//    [Range(1, 10)]
//    public int steps = 1;

//    [Range(.1f, 5)]
//    public float influenceDecay = .5f;

//    [Range(1, 20)]
//    public float verticalShrink = 3;

//    [Range(.1f, 2)]
//    public float multiplier = .5f;

//    [Range(.5f, 2)]
//    public float exponent = 1.1f;

//    #endregion

//    #region PolyTypes
//    public PolyNoiseType polyType;

//    [Range(.1f, 1)]
//    public float persistance = .5f, lacunarity = .5f, frequencyMultiplier = .5f;


//    public NoiseContainer[] polyOperands;
//    #endregion


//    //[OnSerializing]
//    //private void OnSerializing(StreamingContext ctx)
//    //{

//    //}

//    //[OnDeserialized]
//    //private void OnDeserialized(object obj)
//    //{

//    //}

//    public void UpdateInst()
//    {
//        switch (type)
//        {
//            case NoiseOperatorType.Generative:
//                switch (genType)
//                {
//                    case GenerativeNoiseType.Value:
//                        inst = new ValueNoise(seed);
//                        break;
//                    case GenerativeNoiseType.Perlin:
//                        inst = new PerlinNoise(seed);
//                        break;
//                }
//                break;
//            case NoiseOperatorType.Unary:
//                break;
//            case NoiseOperatorType.Binary:
//                break;
//            case NoiseOperatorType.Poly:
//                foreach (var op in polyOperands)
//                {
//                    op.isChild = true;
//                    op.UpdateInst();
//                }

//                switch (polyType)
//                {
//                    case PolyNoiseType.Average:
//                        inst = new AverageFilter(polyOperands);
//                        break;
//                    case PolyNoiseType.Octave:
//                        inst = new OctaveFilter(persistance, lacunarity, polyOperands);
//                break;
//        }
//        break;
//    }
//}

//public float Get(float x, float y, float frequency)
//{
//    return inst.Get(x, y, frequency);
//}
//}

//public enum NoiseOperatorType
//{
//    Generative,
//    Unary,
//    Binary,
//    Poly
//}

//public enum GenerativeNoiseType
//{
//    Value,
//    Perlin
//}

//public enum PolyNoiseType
//{
//    Average,
//    Octave
//}
