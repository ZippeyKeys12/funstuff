using AI;
using Unity.Burst;

namespace Components.AI.Sensors
{
    [BurstCompile]
    public struct SightComponent : ISensorComponent
    {
        public float sightRange;
        public float fov;
    }
}