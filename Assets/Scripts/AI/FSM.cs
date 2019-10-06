using System;
using System.Collections.Generic;
using Structure;
using Unity.Entities;

namespace AI.Reasoners
{
    public class FSM<T> : FSM<Entity, T, IAction>, IReasoner
        where T : Enum, IEquatable<T>
    {
        public string Name { get; }

        public FSM(string name, T defState, IDictionary<T, IAction> states)
            : base((defState, ActionUtil.NULL))
            => Name = name;

        public (IAction state, float confidence) Reason(Entity entity)
            => (Evaluate(entity), .5f);
    }
}