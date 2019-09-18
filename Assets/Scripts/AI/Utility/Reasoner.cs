using System.Collections.Generic;

namespace AI.Utility {
    public class UtilityReasoner : IReasoner {
        protected readonly List<ICurve> curves;

        public UtilityReasoner(List<ICurve> curves) {
            this.curves = curves;
        }

        public bool AddCurve(ICurve curve) {
            if(curves.Contains(curve)) {
                return false;
            }

            curves.Add(curve);

            return true;
        }

        public void Remove(ICurve curve)
            => curves.Remove(curve);

        public void RemoveAt(int index)
            => curves.RemoveAt(index);

        public (AIState, float) Query(AIContext context) {
            var index = 0;
            var max = curves[0].Evaluate(context);

            for(var i = 1; i < curves.Count; i++) {
                var val = curves[i].Evaluate(context);
                if(val > max) {
                    index = i;
                    max = val;
                }
            }

            return (curves[index].State(), max);
        }
    }
}