using System;
using UnityEngine;

namespace AI.Steering {
    public class SteeringState : AIState {
        protected readonly Vector4 target;
        protected readonly SteeringAction action;

        public Vector4 Target
            => target;

        public SteeringAction Action
            => action;

        public SteeringState(SteeringAction action, Vector2 target)
            : this(action, new Vector4(target.x, target.y)) { }

        public SteeringState(SteeringAction action, Vector3 target)
            : this(action, new Vector4(target.x, target.y, target.z)) { }

        public SteeringState(SteeringAction action, Vector4 target)
            : base(Enum.GetName(typeof(SteeringAction), action)) {
            this.action = action;
            this.target = target;
        }
    }

    public enum SteeringAction {
        Seek,
        Flee,
        Pursue,
        Evade,
        Arrive
    }
}