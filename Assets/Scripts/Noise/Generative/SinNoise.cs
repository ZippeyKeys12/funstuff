using System;
using Unity.Mathematics;
using Random = System.Random;

namespace Noise
{
    public class SinNoise : Generator
    {
        protected readonly float offset1d;

        protected readonly float2x2 rotate2d;
        protected readonly float2 translate2d;

        protected readonly float4x4 transform3d;

        public SinNoise(int seed)
        {
            var rnd = new Random(seed);
            Func<float> nextFloat = () => (float)rnd.NextDouble() * 2 * math.PI;

            offset1d = nextFloat();

            rotate2d = float2x2.Rotate(nextFloat());
            translate2d = new float2(nextFloat(), nextFloat());

            transform3d = float4x4.RotateX(nextFloat());
            transform3d *= float4x4.RotateY(nextFloat());
            transform3d *= float4x4.RotateZ(nextFloat());
            transform3d *= float4x4.Translate(new float3(nextFloat(), nextFloat(), nextFloat()));
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;

            return new Sample<float>(math.sin(x + offset1d), math.cos(x + offset1d));
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;

            xy = math.mul(rotate2d, xy);
            xy += offset1d;

            var xay = xy.x + xy.y;
            var xmy = xy.x - xy.y;

            return new Sample<float2>((math.sin(xay) + math.sin(xmy)) / 2,
                (math.cos(xay) + new float2(math.cos(xmy), -math.cos(xmy))) / 2);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;

            var xyzw = math.mul(transform3d, new float4(xyz, 1));

            var xayaz = xyzw.x + xyzw.y + xyzw.z;
            var xmyaz = xyzw.x - xyzw.y + xyzw.z;
            var xmymz = xyzw.x - xyzw.y - xyzw.z;

            return new Sample<float3>((math.sin(xayaz) + math.sin(xmyaz) + math.sin(xmymz)) / 3,
                (math.dot(new float3(1, -1, -1), math.cos(xmymz)) +
                 math.dot(new float3(1, -1, 1), math.cos(xmyaz)) +
                 math.cos(xayaz)) / 3);
        }
    }
}