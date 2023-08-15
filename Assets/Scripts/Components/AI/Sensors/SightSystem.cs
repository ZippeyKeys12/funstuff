using AI;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Components.AI.Sensors
{
    [BurstCompile]
    [UpdateInGroup(typeof(SensorSystemGroup))]
    public class SightSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach(
                (ref SightComponent component, ref Translation translation, ref Rotation rotation) =>
                {
                    throw new System.NotImplementedException();
                })
                .ScheduleParallel();
        }
    }
}