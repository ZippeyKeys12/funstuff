using System;
using UnityEngine;
using Unity.Mathematics;
using TweenType = Interp.TweenType;
using TweenTypes = Interp.TweenTypes;

namespace Noise
{
    [Serializable]
    public class Interpolate : Generator
    {
        private Generator gen;
        private TweenType interp;

        public Interpolate(Generator gen)
            : this(gen, TweenTypes.SmootherStep)
        { }

        public Interpolate(Generator gen, TweenType interp)
        {
            this.gen = gen;
            this.interp = interp;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;
            frequency = 1;

            var i = math.floor(x);

            var d = interp(new Sample<float>(x, 1) - i);

            var s0 = gen.Get(i, frequency);
            var s1 = gen.Get(i + 1, frequency);

            return new Sample<float>(math.lerp(s0.Value, s1.Value, d.Value),
                                     d.Gradient * (s1.Value - s0.Value));
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;
            frequency = 1;

            var i = new int2(math.floor(xy));

            var xr = new Sample<float>(xy.x, 1);
            var yr = new Sample<float>(xy.y, 1);

            var dx = interp(xr - Maths.Floor(xr));
            var dy = interp(yr - Maths.Floor(yr));

            var dxr = new Sample<float2>(dx.Value, new float2(dx.Gradient, 0));
            var dyr = new Sample<float2>(dy.Value, new float2(0, dy.Gradient));

            return Maths.Lerp(Maths.Lerp(gen.Get(i, frequency), gen.Get(i + new float2(1, 0), frequency), dxr),
                              Maths.Lerp(gen.Get(i + new float2(0, 1), frequency), gen.Get(i + new float2(1, 1), frequency), dxr),
                              dyr);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;
            frequency = 1;

            var i = new int3(math.floor(xyz));

            var dx = interp(new Sample<float>(xyz.x, 1));
            var dy = interp(new Sample<float>(xyz.y, 1));
            var dz = interp(new Sample<float>(xyz.z, 1));

            var dxr = new Sample<float3>(dx.Value, new float3(dx.Gradient, 0, 0));
            var dyr = new Sample<float3>(dy.Value, new float3(0, dy.Gradient, 0));
            var dzr = new Sample<float3>(dz.Value, new float3(0, 0, dz.Gradient));

            var j = Maths.Lerp(gen.Get(i, frequency), gen.Get(i + new int3(1, 0, 0), frequency), dxr);
            var k = Maths.Lerp(gen.Get(i + new int3(0, 0, 1), frequency), gen.Get(i + new int3(i + new int3(1, 0, 1)), frequency), dxr);
            var l = Maths.Lerp(gen.Get(i + new int3(0, 1, 0), frequency), gen.Get(i + new int3(1, 1, 0), frequency), dxr);
            var m = Maths.Lerp(gen.Get(i + new int3(0, 1, 1), frequency), gen.Get(i + new int3(1, 1, 1), frequency), dxr);

            return Maths.Lerp(Maths.Lerp(j, l, dyr), Maths.Lerp(k, m, dyr), dzr);
        }
    }
}