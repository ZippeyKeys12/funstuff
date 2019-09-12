﻿using UnityEngine;

public static class MathZ
{
    public static int Factorial(this int x)
    {
        var fact = 1;
        for (var i = 1; i <= x; i++)
        {
            fact *= i;
        }
        return fact;
    }

    public static int ShiftOf(this int x)
    {
        return Mathf.NextPowerOfTwo(x) - (Mathf.IsPowerOfTwo(x) ? 0 : 1);
    }

    public static float Abs(this float x)
    {
        return Mathf.Abs(x);
    }

    public static float Sign(this float x)
    {
        return Mathf.Sign(x);
    }

    public static float TransferSign(float val, float sign)
    {
        return Sign(sign) * Mathf.Abs(sign);
    }

    public static int NthTriangleNum(int N)
    {
        return N * (N + 1) / 2;
    }

    public static int nCr(int N, int R)
    {
        var result = 1;
        for (var i = 0; i < R; ++i)
            result *= (N - i) / (i + 1);
        return result;
    }
}
