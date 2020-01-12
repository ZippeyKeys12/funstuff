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

        public SinNoise()
        { }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;

            return new Sample<float>(math.sin(x + offset1d), math.cos(x + offset1d)) / 2 + .5f;
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;

            var xay = xy.x + xy.y;
            var xmy = xy.x - xy.y;

            return new Sample<float2>((math.sin(xay) + math.sin(xmy)) / 2,
                (math.cos(xay) + new float2(math.cos(xmy), -math.cos(xmy))) / 2) / 2 + .5f;
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;

            var xyzw = new float4(xyz, 1);

            var xayaz = xyzw.x + xyzw.y + xyzw.z;
            var xmyaz = xyzw.x - xyzw.y + xyzw.z;
            var xmymz = xyzw.x - xyzw.y - xyzw.z;

            return new Sample<float3>((math.sin(xayaz) + math.sin(xmyaz) + math.sin(xmymz)) / 3,
                (math.dot(new float3(1, -1, -1), math.cos(xmymz)) +
                 math.dot(new float3(1, -1, 1), math.cos(xmyaz)) +
                 math.cos(xayaz)) / 3) / 2 + .5f;
        }
    }
}