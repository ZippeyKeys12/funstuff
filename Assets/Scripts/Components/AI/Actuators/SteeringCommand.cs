using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Components.AI.Actuators
{
    [BurstCompile]
    public struct SteeringCommand
    {
        public int falloff;
        public float slowingRadius, stoppingRadius;
        public NativeArray<(float3 pos, float3 vel)> targets;
        public SteeringAction action;
    }

    public enum SteeringAction
    {
        Seek,
        Flee,
        Pursue,
        Evade,
        Arrive
    }
}