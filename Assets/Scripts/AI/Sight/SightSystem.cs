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
        struct SightJob : IJobForEach<LocalToWorld, SightComponent>
        {
            public void Execute(ref LocalToWorld c0, ref SightComponent c1)
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