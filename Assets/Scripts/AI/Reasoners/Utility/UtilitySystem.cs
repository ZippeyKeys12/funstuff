using System.Collections.Generic;
using AI.Agent;
using Unity.Entities;

namespace AI.Reasoners.Utility
{
    public class UtilitySystem : IReasoner
    {
        protected readonly string name;
        protected readonly List<ICurve> curves;

        public string Name
            => name;

        public UtilitySystem(string name, List<ICurve> curves)
        {
            this.name = name;
            this.curves = curves;
        }

        public bool AddCurve(ICurve curve)
        {
            if (curves.Contains(curve))
            {
                return false;
            }

            curves.Add(curve);

            return true;
        }

        public void Remove(ICurve curve)
            => curves.Remove(curve);

        public void RemoveAt(int index)
            => curves.RemoveAt(index);

        public (IAction, float) Reason(Entity entity)
        {
            var index = 0;
            var max = curves[0].Evaluate(entity);

            for (var i = 1; i < curves.Count; i++)
            {
                var val = curves[i].Evaluate(entity);
                if (val > max)
                {
                    index = i;
                    max = val;
                }
            }

            return (curves[index].State(), max);
        }
    }
}