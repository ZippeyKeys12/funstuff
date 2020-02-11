using System.Collections.Generic;
using Unity.Mathematics;

namespace Noise
{
    public class NoiseSampler : Sampler
    {
        protected Generator gen;

        public NoiseSampler(Generator gen)
        {
            this.gen = gen;
        }

        public override float[] GetPoints(float x, float freq, int size, int R)
        {
            throw new System.NotImplementedException();
        }

        // Implementation based on Amit Patel's (https://www.redblobgames.com/maps/terrain-from-noise/)
        public override float2[] GetPoints(float2 xy, float freq, int2 size, int R)
        {
            var points = new List<float2>();

            for (var yc = 0; yc < size.y; yc++)
            {

                for (var xc = 0; xc < size.x; xc++)
                {
                    var max = 0f;

                    for (var yn = yc - R; yn <= yc + R; yn++)
                    {
                        for (var xn = xc - R; xn <= xc + R; xn++)
                        {
                            var e = gen.Get(new float2(xn, yn) * freq).Value;

                            if (e > max)
                            {
                                max = e;
                            }
                        }
                    }


                    var c = gen.Get(new float2(xc, yc) * freq).Value;

                    if (c == max)
                    {
                        points.Add(c);
                    }
                }
            }

            return points.ToArray();
        }

        public override float3[] GetPoints(float3 xyz, float freq, int3 size, int R)
        {
            throw new System.NotImplementedException();
        }
    }
}