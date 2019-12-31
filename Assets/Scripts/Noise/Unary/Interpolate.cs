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
        protected float power, invPow;
        protected Generator gen;
        protected TweenType interp;

        public Interpolate(Generator gen)
            : this(gen, TweenTypes.SmootherStep)
        { }

        public Interpolate(Generator gen, TweenType interp)
            : this(gen, interp, 0)
        { }

        public Interpolate(Generator gen, ushort power)
            : this(gen, TweenTypes.SmootherStep, power)
        { }

        public Interpolate(Generator gen, TweenType interp, ushort power)
        {
            this.gen = gen;
            this.interp = interp;
            this.power = math.pow(2, power);
            invPow = 1 / this.power;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;
            frequency = 1;

            var i = math.floor(x * power) / power;

            var xr = new Sample<float>(x, 1) * power;

            var d = interp(xr - Maths.Floor(xr * power) / power);

            return Maths.Lerp(gen.Get(i, frequency), gen.Get(i + invPow, frequency), d);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;
            frequency = 1;

            var i = math.floor(xy * power) / power;

            var xr = new Sample<float>(xy.x, 1);
            var yr = new Sample<float>(xy.y, 1);

            var dx = interp(xr - Maths.Floor(xr * power) / power);
            var dy = interp(yr - Maths.Floor(yr * power) / power);

            var dxr = new Sample<float2>(dx.Value, new float2(dx.Gradient, 0)) * power;
            var dyr = new Sample<float2>(dy.Value, new float2(0, dy.Gradient)) * power;

            return Maths.Lerp(Maths.Lerp(gen.Get(i, frequency), gen.Get(i + new float2(invPow, 0), frequency), dxr),
                              Maths.Lerp(gen.Get(i + new float2(0, invPow), frequency), gen.Get(i + new float2(invPow, invPow), frequency), dxr),
                              dyr);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;
            frequency = 1;

            var i = math.floor(xyz * power) / power;

            var xr = new Sample<float>(xyz.x, 1);
            var yr = new Sample<float>(xyz.y, 1);
            var zr = new Sample<float>(xyz.z, 1);

            var dx = interp(xr - Maths.Floor(xr * power) / power);
            var dy = interp(yr - Maths.Floor(yr * power) / power);
            var dz = interp(zr - Maths.Floor(zr * power) / power);

            var dxr = new Sample<float3>(dx.Value, new float3(dx.Gradient, 0, 0)) * power;
            var dyr = new Sample<float3>(dy.Value, new float3(0, dy.Gradient, 0)) * power;
            var dzr = new Sample<float3>(dz.Value, new float3(0, 0, dz.Gradient)) * power;

            var j = Maths.Lerp(gen.Get(i, frequency), gen.Get(i + new float3(invPow, 0, 0), frequency), dxr);
            var k = Maths.Lerp(gen.Get(i + new float3(0, 0, invPow), frequency), gen.Get(i + new float3(invPow, 0, invPow), frequency), dxr);
            var l = Maths.Lerp(gen.Get(i + new float3(0, invPow, 0), frequency), gen.Get(i + new float3(invPow, invPow, 0), frequency), dxr);
            var m = Maths.Lerp(gen.Get(i + new float3(0, invPow, invPow), frequency), gen.Get(i + new float3(invPow, invPow, invPow), frequency), dxr);

            return Maths.Lerp(Maths.Lerp(j, l, dyr), Maths.Lerp(k, m, dyr), dzr);
        }
    }
}