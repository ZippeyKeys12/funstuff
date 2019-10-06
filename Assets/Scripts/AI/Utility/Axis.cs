using System;
using System.Collections.Generic;
using Unity.Entities;

namespace AI.Reasoners.Utility
{
    public interface ICurve
    {
        IAction State();
        float Evaluate(Entity context);
    }

    public class Axis : ICurve
    {
        protected readonly IAction state;
        protected readonly Func<Entity, float> func;

        public Axis(Func<Entity, float> func)
            : this(null, func) { }

        public Axis(IAction state, Func<Entity, float> func)
        {
            this.state = state;
            this.func = func;
        }

        public IAction State()
            => state;

        public float Evaluate(Entity context)
            => func(context);
    }

    public class MultiAxis : ICurve
    {
        protected readonly IAction state;
        protected readonly List<Axis> axes;

        public MultiAxis(IAction state)
            : this(state, new List<Axis>()) { }

        public MultiAxis(IAction state, List<Axis> axes)
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

        public IAction State()
            => state;

        public float Evaluate(Entity context)
        {
            var score = 1f;
            for (int i = 0; i < axes.Count; i++)
                score *= axes[i].Evaluate(context);

            return score * (1 + (1 - 1 / axes.Count) * (1 - score));
        }
    }
}