using UnityEngine;

namespace AI.Actuators.Teleport
{
    public class TeleportState : AIState
    {
        public Vector3 Pos { get; }
        public Vector3 Vel { get; }

        public TeleportState(string name, Vector3 pos, Vector3 vel)
            : base(name)
        {
            Pos = pos;
            Vel = vel;
        }
    }
}