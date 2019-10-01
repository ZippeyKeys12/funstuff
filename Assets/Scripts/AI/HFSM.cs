using System;
using System.Collections.Generic;
using Structure;

namespace AI.Reasoners
{
    public class HFSM<T> : FSM<AIContext, T, IReasoner>, IReasoner
        where T : Enum, IEquatable<T>
    {
        public string Name { get; }

        public HFSM(string name, T defState, IDictionary<T, AIState> states)
            : base((defState, new Constant("NULL", AIState.NULL, 0)))
            => Name = name;

        public (AIState state, float confidence) Reason(AIContext context)
            => Evaluate(context).Reason(context);
    }
}