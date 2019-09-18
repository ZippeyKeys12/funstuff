using System;

namespace AI {
    public interface IReasoner : INamed {
        (AIState, float) Query(AIContext context);
    }
}