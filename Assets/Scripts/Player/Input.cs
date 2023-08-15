using Unity.Burst;
using Unity.Entities;

namespace Player
{
    [BurstCompile]
    public struct InputComponent : IComponentData
    {
        public float Horizontal, Vertical;
    }
}