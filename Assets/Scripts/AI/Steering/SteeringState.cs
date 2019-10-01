using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Actuators.Steering
{
    public class SteeringState : AIState
    {
        protected readonly int falloff, resXY, resYZ, resXZ;
        protected readonly float maxSpeed, slowingRadius, stoppingRadius;
        protected readonly Vector3 pos;
        protected readonly (Vector3 pos, Vector3 vel)[] targets;
        protected readonly SteeringAction action;

        public int ResXY
            => resXY;

        public int ResYZ
            => resYZ;

        public int ResXZ
            => resXZ;

        public int Falloff
            => falloff;

        public float MaxSpeed
            => maxSpeed;

        public float SlowingRadius
            => slowingRadius;

        public float StoppingRadius
            => stoppingRadius;

        public Vector3 Pos
            => pos;

        public (Vector3 pos, Vector3 vel)[] Targets
            => targets.ToArray();

        public SteeringAction Action
            => action;

        public SteeringState(SteeringAction action, Vector3 pos, (Vector3 pos, Vector3 vel)[] targets,
            float maxSpeed, int resXY, int resYZ, int resXZ,
            float slowingRadius = 0, float stoppingRadius = 0, int falloff = 0)
            : base(Enum.GetName(typeof(SteeringAction), action))
        {
            this.pos = pos;
            this.action = action;
            this.targets = targets;
            this.maxSpeed = maxSpeed;

            this.resXY = resXY;
            this.resYZ = resYZ;
            this.resXZ = resXZ;

            this.slowingRadius = slowingRadius;
            this.stoppingRadius = stoppingRadius;
            this.falloff = falloff;
        }
    }

    public enum SteeringAction
    {
        Seek,
        Flee,
        Pursue,
        Evade,
        Arrive
    }
}