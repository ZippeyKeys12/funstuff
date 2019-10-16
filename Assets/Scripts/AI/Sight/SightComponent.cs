using Unity.Burst;
using Unity.Entities;

namespace AI.Sensors.Sight
{
    [BurstCompile]
    public struct SightComponent : ISensorComponent
    {
        public float sightRange;
        public float fov;
    }
}