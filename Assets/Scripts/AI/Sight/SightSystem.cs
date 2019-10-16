using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace AI.Sensors.Sight
{
    [UpdateInGroup(typeof(SensorSystemGroup))]
    public class SightSystem : JobComponentSystem
    {
        [BurstCompile]
        struct SightJob : IJobForEach<SightComponent, Translation, Rotation>
        {
            public void Execute(ref SightComponent component, ref Translation translation, ref Rotation rotation)
            {
                throw new System.NotImplementedException();
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new SightJob().Schedule(this, inputDeps);
        }
    }
}