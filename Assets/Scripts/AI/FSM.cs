using System;
using System.Collections.Generic;
using Structure;

namespace AI.Reasoners
{
    public class FSM<T> : FSM<AIContext, T, AIState>, IReasoner
        where T : Enum, IEquatable<T>
    {
        public string Name { get; }

        public FSM(string name, T defState, IDictionary<T, AIState> states)
            : base((defState, AIState.NULL))
        {
            Name = name;
        }

        public (AIState state, float confidence) Reason(AIContext context)
            => (Evaluate(context), .5f);
    }
}