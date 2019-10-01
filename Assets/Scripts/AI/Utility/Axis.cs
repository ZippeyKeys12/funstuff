using System;
using System.Collections.Generic;

namespace AI.Reasoners.Utility
{
    public interface ICurve
    {
        AIState State();
        float Evaluate(AIContext context);
    }

    public class Axis : ICurve
    {
        protected readonly AIState state;
        protected readonly Func<AIContext, float> func;

        public Axis(Func<AIContext, float> func)
            : this(null, func) { }

        public Axis(AIState state, Func<AIContext, float> func)
        {
            this.state = state;
            this.func = func;
        }

        public AIState State()
            => state;

        public float Evaluate(AIContext context)
            => func(context);
    }

    public class MultiAxis : ICurve
    {
        protected readonly AIState state;
        protected readonly List<Axis> axes;

        public MultiAxis(AIState state)
            : this(state, new List<Axis>()) { }

        public MultiAxis(AIState state, List<Axis> axes)
        {
            this.state = state;
            this.axes = axes;
        }

        public bool Add(Axis axis)
        {
            if (axes.Contains(axis))
            {
                return false;
            }

            axes.Add(axis);

            return true;
        }

        public AIState State()
            => state;

        public float Evaluate(AIContext context)
        {
            var score = 1f;
            for (int i = 0; i < axes.Count; i++)
                score *= axes[i].Evaluate(context);

            return score * (1 + (1 - 1 / axes.Count) * (1 - score));
        }
    }
}