using System;
using System.Collections.Generic;
using Structure;
using Unity.Entities;

namespace AI.Reasoners
{
    public class HFSM<T> : FSM<Entity, T, IReasoner>, IReasoner
        where T : Enum, IEquatable<T>
    {
        public string Name { get; }

        public HFSM(string name, T defState, IDictionary<T, IAction> states)
            : base((defState, new Constant("NULL", ActionUtil.NULL, 0)))
            => Name = name;

        public (IAction state, float confidence) Reason(Entity entity)
            => Evaluate(entity).Reason(entity);
    }
}