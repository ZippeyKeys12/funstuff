using System.Linq;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace AI.Actuators.Teleport
{
    public class Teleport : IActuator
    {
        public string Name { get; }

        public void Act(Entity entity, AIState[] states)
        {
            var state = states.OfType<TeleportState>().Last();

            if (state != null)
            {
                var translation = World.Active.EntityManager.GetComponentData<Translation>(entity);
                translation.Value = state.Pos;

                var rotation = World.Active.EntityManager.GetComponentData<Rotation>(entity);
                rotation.Value = new Quaternion(); // TODO: Finish
            }
        }
    }
}