using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using AI;
namespace Components.AI.Actuators.Steering
{
    [BurstCompile]
    public struct SteeringComponent : IActuatorComponent
    {
        public int resXY, resYZ, resXZ;
        public float maxSpeed, maxSteering;
        public DynamicBuffer<SteeringCommand> commands;
    }
}