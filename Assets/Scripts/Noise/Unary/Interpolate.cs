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
        protected float unit, invUnit;
        protected Generator gen;
        protected TweenType interp;

        public Interpolate(Generator gen)
            : this(gen, TweenTypes.SmootherStep)
        { }

        public Interpolate(Generator gen, TweenType interp)
            : this(gen, interp, 1)
        { }

        public Interpolate(Generator gen, float unit)
            : this(gen, TweenTypes.SmootherStep, unit)
        { }

        public Interpolate(Generator gen, TweenType interp, float unit)
        {
            this.gen = gen;
            this.interp = interp;
            this.unit = unit;
            invUnit = 1 / unit;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;
            frequency = 1;

            var i = math.floor(x * invUnit) / invUnit;

            var xr = new Sample<float>(x, 1) * invUnit;

            var d = interp(xr - Maths.Floor(xr * invUnit) / invUnit);

            return Maths.Lerp(gen.Get(i, frequency), gen.Get(i + unit, frequency), d);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;
            frequency = 1;

            var i = math.floor(xy * invUnit) / invUnit;

            var xr = new Sample<float>(xy.x, 1);
            var yr = new Sample<float>(xy.y, 1);

            var dx = interp(xr - Maths.Floor(xr * invUnit) / invUnit);
            var dy = interp(yr - Maths.Floor(yr * invUnit) / invUnit);

            var dxr = new Sample<float2>(dx.Value, new float2(dx.Gradient, 0)) * invUnit;
            var dyr = new Sample<float2>(dy.Value, new float2(0, dy.Gradient)) * invUnit;

            return Maths.Lerp(Maths.Lerp(gen.Get(i, frequency), gen.Get(i + new float2(unit, 0), frequency), dxr),
                              Maths.Lerp(gen.Get(i + new float2(0, unit), frequency), gen.Get(i + new float2(unit, unit), frequency), dxr),
                              dyr);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            xyz *= frequency;
            frequency = 1;

            var i = math.floor(xyz * invUnit) / invUnit;

            var xr = new Sample<float>(xyz.x, 1);
            var yr = new Sample<float>(xyz.y, 1);
            var zr = new Sample<float>(xyz.z, 1);

            var dx = interp(xr - Maths.Floor(xr * invUnit) / invUnit);
            var dy = interp(yr - Maths.Floor(yr * invUnit) / invUnit);
            var dz = interp(zr - Maths.Floor(zr * invUnit) / invUnit);

            var dxr = new Sample<float3>(dx.Value, new float3(dx.Gradient, 0, 0)) * invUnit;
            var dyr = new Sample<float3>(dy.Value, new float3(0, dy.Gradient, 0)) * invUnit;
            var dzr = new Sample<float3>(dz.Value, new float3(0, 0, dz.Gradient)) * invUnit;

            var j = Maths.Lerp(gen.Get(i, frequency), gen.Get(i + new float3(unit, 0, 0), frequency), dxr);
            var k = Maths.Lerp(gen.Get(i + new float3(0, 0, unit), frequency), gen.Get(i + new float3(unit, 0, unit), frequency), dxr);
            var l = Maths.Lerp(gen.Get(i + new float3(0, unit, 0), frequency), gen.Get(i + new float3(unit, unit, 0), frequency), dxr);
            var m = Maths.Lerp(gen.Get(i + new float3(0, unit, unit), frequency), gen.Get(i + new float3(unit, unit, unit), frequency), dxr);

            return Maths.Lerp(Maths.Lerp(j, l, dyr), Maths.Lerp(k, m, dyr), dzr);
        }
    }
}