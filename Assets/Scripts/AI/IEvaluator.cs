using System.Collections.Generic;

namespace AI {
    public interface IEvaluator {
        List<AIState> Evaluate(List<(AIState, float)> ideas);
    }
}