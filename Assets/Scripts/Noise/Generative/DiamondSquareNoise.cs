using System;
using UnityEngine;
using Random = System.Random;

//public class DiamondSquareNoise : INoise
//{
//    public int steps;
//    public float mult, exp;

//    public override void GenerateNoise()
//    {
//        var rng = new Random(seed);
//        Func<float> rnd = () => (float)rng.NextDouble();

//        actualNoise = ImageUtils.DiamondSquare(new float[,] { { rnd(), rnd() }, { rnd(), rnd() } }, rnd, steps, mult, exp);
//        Debug.Log($"{actualNoise.GetLength(0)} x {actualNoise.GetLength(1)}");
//    }

//    protected override float GeneratePoint(float x, float y)
//    {
//        throw new NotImplementedException();
//    }

//    public override float AtXY(float x, float y)
//    {
//        return actualNoise[Mathf.Clamp(Mathf.RoundToInt(x), 0, actualNoise.GetLength(0) - 1), Mathf.Clamp(Mathf.RoundToInt(y), 0, actualNoise.GetLength(1) - 1)];

//    }
//}
