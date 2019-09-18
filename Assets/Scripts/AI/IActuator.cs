using System.Collections.Generic;

namespace AI {
    public interface IActuator : INamed {
        void Act(AIContext context, List<AIState> states);
    }
}