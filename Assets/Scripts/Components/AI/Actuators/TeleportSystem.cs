using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

namespace Components.AI.Actuators
{
    public class TeleportSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref TeleportComponent component, ref Translation translation, ref Rotation rotation,
                ref PhysicsVelocity velocity) =>
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
            }).ScheduleParallel();
        }
    }
}