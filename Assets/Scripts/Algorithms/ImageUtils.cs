using System;
using Unity.Mathematics;

public static class ImageUtils
{
    public static float[,] DiamondSquare(float[,] initial, Func<float> rng, int steps, float mult, float exp)
    {
        float[,] result = null;

        float f;
        for (var n = 0; n < steps; n++)
        {
            f = 2 * mult;

            Func<float> rnd;
            if (n + 1 < steps)
            {
                rnd = () => (float)rng() * f - mult;
            }
            else
            {
                rnd = () => 0;
            }


            int w = initial.GetLength(0),
                h = initial.GetLength(1);

            result = new float[w, h];

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    result[x, y] = ((x & 1) | (y & 1)) != 0 ? 0 : initial[x >> 1, y >> 1];
                }
            }

            for (var x = 1; x < 2 * w - 1; x += 2)
            {
                for (var y = 1; y < 2 * h - 1; y += 2)
                {
                    var t = result[x, y] = (result[x - 1, y - 1] + result[x + 1, y - 1] + result[x - 1, y + 1] + result[x + 1, y + 1]) / 4 + rnd();
                    result[x, y - 1] += t;
                    result[x + 1, y] += t;
                    result[x, y + 1] += t;
                    result[x - 1, y] += t;
                }
            }

            for (var x = 1; x < 2 * w - 1; x += 2)
            {
                for (var y = 0; y < 2 * h - 1; y += 2)
                {
                    var d = y > 0 && y < 2 * h - 2 ? 4 : 3;
                    result[x, y] = (result[x, y] + result[x - 1, y] + result[x + 1, y]) / d + rnd();
                    result[y, x] = (result[y, x] + result[y, x - 1] + result[y, x + 1]) / d + rnd();
                }
            }

            initial = result;
            mult /= math.pow(2, exp);
        }

        return result;
    }
}
