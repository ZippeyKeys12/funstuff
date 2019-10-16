using Unity.Burst;
using Unity.Entities;
using AI;
namespace Components.AI.Sensors
{
    [BurstCompile]
    public struct SightComponent : ISensorComponent
    {
        public float sightRange;
        public float fov;
    }
}