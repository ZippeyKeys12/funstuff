using System;
using System.Collections.Generic;
using Structure;

namespace AI.Reasoners
{
    public class FSM<T> : FSM<AIContext, T, IAction>, IReasoner
        where T : Enum, IEquatable<T>
    {
        public string Name { get; }

        public FSM(string name, T defState, IDictionary<T, IAction> states)
            : base((defState, ActionUtil.NULL))
            => Name = name;

        public (IAction state, float confidence) Reason(AIContext context)
            => (Evaluate(context), .5f);
    }
}