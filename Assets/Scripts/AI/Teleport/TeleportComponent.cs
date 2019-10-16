using Unity.Burst;
using Unity.Mathematics;

namespace AI.Actuators.Teleport
{
    [BurstCompile]
    public struct TeleportComponent : IActuatorComponent
    {
        public bool useTranslation;
        public float3 translation;

        public bool useRotation;
        public quaternion rotation;

        public bool useLinearVelocity;
        public float3 linearVelocity;

        public bool useAngularVelocity;
        public float3 angularVelocity;
    }
}