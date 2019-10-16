using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

namespace AI.Actuators.Steering
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