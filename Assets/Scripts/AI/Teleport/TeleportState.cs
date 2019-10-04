using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

namespace AI.Actuators.Teleport
{
    public class TeleportState : IAction, IComponentData
    {
        public Vector3 Pos { get; }
        public Vector3 Vel { get; }

        public TeleportState(string name, Vector3 pos, Vector3 vel)
        {
            Pos = pos;
            Vel = vel;
        }
    }
}