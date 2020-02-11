using System;
using Unity.Mathematics;
using TweenType = Interp.TweenType;
using TweenTypes = Interp.TweenTypes;

namespace Noise
{
    [Serializable]
    public class Interpolate : Generator
    {
        protected float unit;
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
        }

        public override Sample<float> Get(float x)
        {
            var i = math.floor(x / unit) * unit;

            var xr = new Sample<float>(x, 1) / unit;

            var d = interp(xr - (Maths.Floor(xr / unit) * unit));

            return Maths.Lerp(gen.Get(i), gen.Get(i + unit), d);
        }

        public override Sample<float2> Get(float2 xy)
        {
            var i = math.floor(xy / unit) * unit;

            var xr = new Sample<float>(xy.x, 1);
            var yr = new Sample<float>(xy.y, 1);

            var dx = interp(xr - (Maths.Floor(xr / unit) * unit));
            var dy = interp(yr - (Maths.Floor(yr / unit) * unit));

            var dxr = new Sample<float2>(dx.Value, new float2(dx.Gradient, 0)) / unit;
            var dyr = new Sample<float2>(dy.Value, new float2(0, dy.Gradient)) / unit;

            return Maths.Lerp(Maths.Lerp(gen.Get(i), gen.Get(i + new float2(unit, 0)), dxr),
                              Maths.Lerp(gen.Get(i + new float2(0, unit)), gen.Get(i + new float2(unit, unit)), dxr),
                              dyr);
        }

        public override Sample<float3> Get(float3 xyz)
        {
            var i = math.floor(xyz / unit) * unit;

            var xr = new Sample<float>(xyz.x, 1);
            var yr = new Sample<float>(xyz.y, 1);
            var zr = new Sample<float>(xyz.z, 1);

            var dx = interp(xr - (Maths.Floor(xr / unit) * unit));
            var dy = interp(yr - (Maths.Floor(yr / unit) * unit));
            var dz = interp(zr - (Maths.Floor(zr / unit) * unit));

            var dxr = new Sample<float3>(dx.Value, new float3(dx.Gradient, 0, 0)) / unit;
            var dyr = new Sample<float3>(dy.Value, new float3(0, dy.Gradient, 0)) / unit;
            var dzr = new Sample<float3>(dz.Value, new float3(0, 0, dz.Gradient)) / unit;

            var j = Maths.Lerp(gen.Get(i), gen.Get(i + new float3(unit, 0, 0)), dxr);
            var k = Maths.Lerp(gen.Get(i + new float3(0, 0, unit)), gen.Get(i + new float3(unit, 0, unit)), dxr);
            var l = Maths.Lerp(gen.Get(i + new float3(0, unit, 0)), gen.Get(i + new float3(unit, unit, 0)), dxr);
            var m = Maths.Lerp(gen.Get(i + new float3(0, unit, unit)), gen.Get(i + new float3(unit, unit, unit)), dxr);

            return Maths.Lerp(Maths.Lerp(j, l, dyr), Maths.Lerp(k, m, dyr), dzr);
        }
    }
}