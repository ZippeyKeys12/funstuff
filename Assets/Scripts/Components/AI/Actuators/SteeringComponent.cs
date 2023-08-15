using AI;
using Unity.Burst;
using Unity.Entities;

namespace Components.AI.Actuators
{
    [BurstCompile]
    public struct SteeringComponent : IActuatorComponent
    {
        public int resXY, resYZ, resXZ;
        public float maxSpeed, maxSteering;
        public DynamicBuffer<SteeringCommand> commands;
    }
}