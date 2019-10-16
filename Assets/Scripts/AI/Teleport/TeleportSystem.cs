using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

namespace Components.AI.Actuators
{
    public class TeleportSystem : JobComponentSystem
    {
        [BurstCompile]
        struct TeleportJob : IJobForEach<TeleportComponent, Translation, Rotation, PhysicsVelocity>
        {
            public void Execute(ref TeleportComponent component, ref Translation translation, ref Rotation rotation, ref PhysicsVelocity velocity)
            {
                if (component.useTranslation)
                {
                    translation.Value = component.translation;
                }

                if (component.useRotation)
                {
                    rotation.Value = component.rotation;
                }

                if (component.useLinearVelocity)
                {
                    velocity.Linear = component.linearVelocity;
                }

                if (component.useAngularVelocity)
                {
                    velocity.Angular = component.angularVelocity;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new TeleportJob().Schedule(this, inputDeps);
        }
    }
}