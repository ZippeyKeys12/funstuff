using System;
using Unity.Mathematics;
using UnityEngine;

namespace Noise
{
    public class MidpointDisplacement : Generator
    {
        protected int divisions;
        protected float windowSize, unit, invUnit;
        protected Generator seed, jitter;

        public MidpointDisplacement(Generator seed, Generator jitter, float windowSize, int divisions)
        {
            this.seed = seed;
            this.jitter = jitter;

            this.windowSize = windowSize;

            this.divisions = divisions;
            unit = windowSize / math.pow(2, divisions);
            invUnit = 1 / unit;
        }

        public override Sample<float> Get(float x, float frequency)
        {
            x *= frequency;
            frequency = 1;

            var i = math.floor(x / unit) * unit;

            var leftIndex = math.floor(x / windowSize) * windowSize;
            var leftValue = seed.Get(leftIndex, frequency).Value;

            var rightIndex = leftIndex + windowSize;
            var rightValue = seed.Get(rightIndex, frequency).Value;

            var result = midDisp1d(i, frequency, (t, f) => jitter.Get(t, f).Value, leftIndex, leftValue, rightIndex, rightValue);

            if (result == float.NaN)
            {
                return null;
            }

            return new Sample<float>(result);
        }

        public override Sample<float2> Get(float2 xy, float frequency)
        {
            xy *= frequency;
            frequency = 1;

            var i = math.floor(xy / unit) * unit;

            var topLeftIndex = math.floor(xy / windowSize) * windowSize;
            var topLeftValue = seed.Get(topLeftIndex, frequency).Value;

            var topRightIndex = topLeftIndex + new float2(windowSize, 0);
            var topRightValue = seed.Get(topRightIndex, frequency).Value;

            var bottomLeftIndex = topLeftIndex + new float2(0, windowSize);
            var bottomLeftValue = seed.Get(bottomLeftIndex, frequency).Value;

            var bottomRightIndex = topLeftIndex + new float2(windowSize, windowSize);
            var bottomRightValue = seed.Get(bottomRightIndex, frequency).Value;

            var result = midDisp2d(i, frequency, (t, f) => jitter.Get(t, f).Value, topLeftIndex, topLeftValue, topRightIndex, topRightValue, bottomLeftIndex, bottomLeftValue, bottomRightIndex, bottomRightValue);

            if (result == float.NaN)
            {
                return null;
            }

            return new Sample<float2>(result);
        }

        public override Sample<float3> Get(float3 xyz, float frequency)
        {
            throw new System.NotImplementedException();
        }

        protected float midDisp1d(float target, float frequency, Func<float, float, float> getter,
            float leftIndex, float leftValue, float rightIndex, float rightValue)
        {
            if (target == leftIndex)
            {
                return leftValue;
            }

            if (target == rightIndex)
            {
                return rightValue;
            }

            for (var i = 0; i < divisions; i++)
            {
                var centerIndex = (leftIndex + rightIndex) / 2;
                var centerValue = (leftValue + rightValue) / 2 + getter(centerIndex, frequency);

                if (target == centerIndex)
                {
                    return centerValue;
                }

                if (target < centerIndex)
                {
                    rightIndex = centerIndex;
                    rightValue = centerValue;
                }

                else
                {
                    leftIndex = centerIndex;
                    leftValue = centerValue;
                }
            }

            return float.NaN;
        }

        protected float midDisp2d(float2 target, float frequency, Func<float2, float, float> getter,
            float2 topLeftIndex, float topLeftValue, float2 topRightIndex, float topRightValue,
            float2 bottomLeftIndex, float bottomLeftValue, float2 bottomRightIndex, float bottomRightValue)
        {
            if (target.Equals(topLeftIndex))
            {
                return topLeftValue;
            }

            if (target.Equals(topRightIndex))
            {
                return topRightValue;
            }

            if (target.Equals(bottomLeftIndex))
            {
                return bottomLeftValue;
            }

            if (target.x == topLeftIndex.x)
            {
                return midDisp1d(target.y, frequency, (y, f) => getter(new float2(target.x, y), f), topLeftIndex.y, topLeftValue, bottomLeftIndex.y, bottomLeftValue);
            }

            if (target.y == topLeftIndex.y)
            {
                return midDisp1d(target.x, frequency, (x, f) => getter(new float2(x, target.y), f), topLeftIndex.x, topLeftValue, topRightIndex.x, topRightValue);
            }

            if (target.Equals(bottomRightIndex))
            {
                return bottomRightValue;
            }

            if (target.x == bottomRightIndex.x)
            {
                return midDisp1d(target.y, frequency, (y, f) => getter(new float2(target.x, y), f),
                    topRightIndex.x, topRightValue, bottomRightIndex.y, bottomRightValue);
            }

            if (target.y == bottomRightIndex.y)
            {
                return midDisp1d(target.x, frequency, (x, f) => getter(new float2(x, target.y), f),
                    bottomLeftIndex.y, bottomLeftValue, bottomRightIndex.y, bottomRightValue);
            }

            for (var i = 0; i < divisions; i++)
            {
                var centerLeftIndex = (topLeftIndex + bottomLeftIndex) / 2;
                var centerLeftValue = (topLeftValue + bottomLeftValue) / 2 + getter(centerLeftIndex, frequency);

                var centerTopIndex = (topLeftIndex + topRightIndex) / 2;
                var centerTopValue = (topLeftValue + topRightValue) / 2 + getter(centerTopIndex, frequency);

                var centerRightIndex = (topRightIndex + bottomRightIndex) / 2;
                var centerRightValue = (topRightValue + bottomRightValue) / 2 + getter(centerRightIndex, frequency);

                var centerBottomIndex = (bottomLeftIndex + bottomRightIndex) / 2;
                var centerBottomValue = (bottomLeftValue + bottomRightValue) / 2 + getter(centerBottomIndex, frequency);

                var centerIndex = new float2(centerTopIndex.x, centerLeftIndex.y);
                var centerValue = (centerLeftValue + centerTopValue + centerRightValue + centerBottomValue) / 4;

                if (target.Equals(centerIndex))
                {
                    return centerValue;
                }

                if (target.x == centerIndex.x)
                {
                    if (target.y < centerIndex.y)
                    {
                        return midDisp1d(target.y, frequency, (y, f) => getter(new float2(target.x, y), f),
                            centerTopIndex.y, centerTopValue, centerIndex.y, centerValue);
                    }
                    else if (target.y > centerIndex.y)
                    {
                        return midDisp1d(target.y, frequency, (y, f) => getter(new float2(target.x, y), f),
                            centerIndex.y, centerValue, centerBottomIndex.y, centerBottomValue);
                    }
                }
                else if (target.y == centerIndex.y)
                {
                    if (target.x < centerIndex.x)
                    {
                        return midDisp1d(target.x, frequency, (x, f) => getter(new float2(x, target.y), f),
                            centerLeftIndex.x, centerLeftValue, centerIndex.x, centerValue);
                    }
                    else if (target.x > centerIndex.x)
                    {
                        return midDisp1d(target.x, frequency, (x, f) => getter(new float2(x, target.y), f),
                            centerIndex.x, centerValue, centerRightIndex.x, centerRightValue);
                    }
                }

                if (target.x < centerIndex.x)
                {
                    if (target.y < centerIndex.y)
                    {
                        bottomLeftIndex = centerLeftIndex;
                        bottomLeftValue = centerLeftValue;

                        bottomRightIndex = centerIndex;
                        bottomRightValue = centerValue;

                        topRightIndex = centerTopIndex;
                        topRightValue = centerTopValue;
                    }
                    else if (target.y > centerIndex.y)
                    {
                        topLeftIndex = centerLeftIndex;
                        topLeftValue = centerLeftValue;

                        topRightIndex = centerIndex;
                        topRightValue = centerValue;

                        bottomRightIndex = centerBottomIndex;
                        bottomRightValue = centerBottomValue;
                    }
                }
                else if (target.x > centerIndex.x)
                {
                    if (target.y < centerIndex.y)
                    {
                        topLeftIndex = centerTopIndex;
                        topLeftValue = centerTopValue;

                        bottomLeftIndex = centerIndex;
                        bottomLeftValue = centerValue;

                        bottomRightIndex = centerRightIndex;
                        bottomRightValue = centerRightValue;
                    }
                    else if (target.y > centerIndex.y)
                    {
                        bottomLeftIndex = centerBottomIndex;
                        bottomLeftValue = centerBottomValue;

                        topLeftIndex = centerIndex;
                        topLeftValue = centerValue;

                        topRightIndex = centerRightIndex;
                        topRightValue = centerRightValue;
                    }
                }
            }

            return float.NaN;
        }
    }
}