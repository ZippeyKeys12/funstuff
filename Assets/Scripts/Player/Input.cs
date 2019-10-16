using Unity.Burst;
using Unity.Entities;

namespace Player.Control
{
    [BurstCompile]
    public struct InputComponent : IComponentData
    {
        public float Horizontal, Vertical;
    }
}