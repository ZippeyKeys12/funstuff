using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using AI;
namespace Components.AI.Actuators.Steering
{
    // TODO: Test
    [UpdateInGroup(typeof(ActuatorSystemGroup))]
    public class SteeringSystem : JobComponentSystem
    {
        // [BurstCompile]
        struct SteeringJob : IJobForEach<SteeringComponent, LocalToWorld, PhysicsVelocity>
        {
            public void Execute(ref SteeringComponent component, ref LocalToWorld localToWorld, ref PhysicsVelocity velocity)
            {
                var resXY = component.resXY;
                var resYZ = component.resYZ;
                var resXZ = component.resXZ;

                int res = 4 * resXY + 4 * resYZ + 4 * resXZ;
                // switch (new int[] { resXY, resYZ, resXZ }.Where(x => x > 0).Count())
                // {
                //     case 2:
                //         res -= 2;
                //         break;
                //     case 3:
                //         res -= 6;
                //         break;
                //     default: break;
                // }

                if (resXY > 0 && resYZ > 0 && resXZ > 0)
                {
                    res -= 6;
                }
                else if ((resXY > 0 && resYZ > 0) || (resXY > 0 && resXZ > 0) || (resXZ > 0 && resYZ > 0))
                {
                    res -= 6;
                }

                var basis = new NativeArray<float3>(res, Allocator.Temp);
                var index = 0;

                if (resXY != 0)
                {
                    for (var i = 0f; i < 2 * math.PI; i += math.PI / (2 * resXY))
                    {
                        basis[index++] = new float3(math.cos(i), math.sin(i), 0);
                    }
                }

                if (resYZ != 0)
                {
                    for (var i = 0f; i < 2 * math.PI; i += math.PI / (2 * resYZ))
                    {
                        if (resXY == 0 || (i != math.PI / 2 && i != 3 * math.PI / 2))
                        {
                            basis[index++] = new float3(0, math.sin(i), math.cos(i));
                        }
                    }
                }

                if (resXZ != 0)
                {
                    for (var i = 0f; i < 2 * math.PI; i += math.PI / (2 * resXZ))
                    {
                        if ((resXY == 0 || (i != math.PI / 2 && i != 3 * math.PI / 2))
                         && (resYZ == 0 || (i != 0 && i != math.PI)))
                        {
                            basis[index++] = new float3(math.sin(i), 0, math.cos(i));
                        }
                    }
                }

                var interestMap = new NativeArray<float>(basis.Length, Allocator.Temp);
                var dangerMap = new NativeArray<float>(basis.Length, Allocator.Temp);

                foreach (var command in component.commands)
                {
                    var action = command.action;
                    var falloff = command.falloff;
                    var pos = localToWorld.Position;

                    SteeringType type;
                    var idealVec = float3.zero;
                    if (action == SteeringAction.Seek)
                    {
                        type = SteeringType.Interest;
                        foreach (var target in command.targets.Select((x, i) => (i, x.pos, x.vel)))
                        {
                            idealVec = Seek(pos, target.pos, 1 / math.pow(2, target.i));
                        }
                    }

                    else if (action == SteeringAction.Flee)
                    {
                        type = SteeringType.Danger;
                        foreach (var target in command.targets.Select((x, i) => (i, x.pos, x.vel)))
                        {
                            idealVec = Flee(pos, target.pos, 1 / math.pow(2, target.i));
                        }
                    }

                    else if (action == SteeringAction.Pursue)
                    {
                        type = SteeringType.Interest;
                        foreach (var target in command.targets.Select((x, i) => (i, x.pos, x.vel)))
                        {
                            idealVec = Pursue(pos, target.pos, target.vel, 1 / math.pow(2, target.i));
                        }
                    }

                    else if (action == SteeringAction.Evade)
                    {
                        type = SteeringType.Danger;
                        foreach (var target in command.targets.Select((x, i) => (i, x.pos, x.vel)))
                        {
                            idealVec = Evade(pos, target.pos, target.vel, 1 / math.pow(2, target.i));
                        }
                    }

                    else if (action == SteeringAction.Arrive)
                    {
                        type = SteeringType.Interest;
                        foreach (var target in command.targets.Select((x, i) => (i, x.pos, x.vel)))
                        {
                            idealVec = Arrive(pos, target.pos, 1 / math.pow(2, target.i), command.slowingRadius, command.stoppingRadius);
                        }
                    }

                    else
                    {
                        throw new NotImplementedException(
                            $"{Enum.GetName(typeof(SteeringAction), action)} isn't handled by {typeof(SteeringSystem).FullName}");
                    }

                    var contextMap = type == SteeringType.Interest ? interestMap : dangerMap;
                    var closestDir = basis.Select((x, ind) => (ind, value: math.dot(idealVec, x)))
                                      .Aggregate((a, b) => a.value > b.value ? a : b)
                                      .ind;

                    for (int i = 0; i <= falloff; i++)
                    {
                        var top = wraparoundIndex(contextMap, closestDir + falloff);
                        var bottom = wraparoundIndex(contextMap, closestDir - falloff);
                        contextMap[top] = math.dot(idealVec, basis[top]) / math.pow(2, i);
                        contextMap[bottom] = math.dot(idealVec, basis[bottom]) / math.pow(2, i);
                    }
                }

                /* Evaluate context map */
                var angle = basis[0];
                var max = (dangerMap[0] == 0) ? interestMap[0] : 0;
                index = 0;
                var cleanedMap = new NativeArray<float>(basis.Length, Allocator.Temp);
                for (var i = 0; i < basis.Length; i++)
                {
                    if (dangerMap[i] == 0)
                    {
                        cleanedMap[i] = interestMap[i];

                        if (cleanedMap[i] > max)
                        {
                            angle = basis[i];
                            max = cleanedMap[i];
                            index = i;
                        }
                    }
                }
                cleanedMap.Dispose();

                /* Choose side to weight */
                var plus1 = cleanedMap[wraparoundIndex(cleanedMap, index + 1)];
                var minus1 = cleanedMap[wraparoundIndex(cleanedMap, index - 1)];
                float ia = 0, ib = 0, ic = 0, id = 0;
                if (plus1 <= minus1)
                {
                    ia = cleanedMap[wraparoundIndex(cleanedMap, index - 2)];
                    ib = minus1;
                    ic = max;
                    id = plus1;

                    if (ia > ib)
                    {
                        ia = ib = ic = id = 0;
                    }
                }
                else
                {
                    ia = minus1;
                    ib = max;
                    ic = plus1;
                    id = cleanedMap[wraparoundIndex(cleanedMap, index + 2)];

                    if (id > ic)
                    {
                        ia = ib = ic = id = 0;
                    }
                }

                /* Blend results */
                if (ia + id != ib + ic && (ia != 0 || ib != 0) && (ic != 0 || id != 0))
                {
                    var t = (ia - 2 * ib + ic) / (ia + id - (ib + ic));

                    var floored = (int)math.floor(t) - index;
                    var ceiled = (int)math.ceil(t) - index;
                    angle = math.lerp(basis[wraparoundIndex(cleanedMap, floored)], basis[wraparoundIndex(cleanedMap, ceiled)], t % 1);
                    max = (id - ic) * t + ic;
                }

                velocity.Linear += component.maxSteering * math.normalize(component.maxSpeed * angle - velocity.Linear);

                /* Cleanup */
                // component.commands.Dispose();
                interestMap.Dispose(); //TODO: Hysteresis
                dangerMap.Dispose();
                basis.Dispose();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int wraparoundIndex(NativeArray<float> contextMap, int index)
                => index < 0 ? wraparoundIndex(contextMap, contextMap.Length + index) : index % contextMap.Length;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float3 Seek(float3 agentPos, float3 target, float maxSpeed)
                => maxSpeed * math.normalize(target - agentPos);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float3 Flee(float3 agentPos, float3 target, float maxSpeed)
                => -Seek(agentPos, target, maxSpeed);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float3 Pursue(float3 agentPos, float3 target, float3 vel, float maxSpeed)
                => Seek(agentPos, target + vel, maxSpeed);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float3 Evade(float3 agentPos, float3 target, float3 vel, float maxSpeed)
                => -Pursue(agentPos, target, vel, maxSpeed);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float3 Arrive(float3 agentPos, float3 target, float maxSpeed, float slowingRadius, float stoppingRadius = 0)
            {
                var diff = target - agentPos;
                return maxSpeed * math.unlerp(stoppingRadius, slowingRadius, math.length(diff)) * math.normalize(diff);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return new SteeringJob().Schedule(this, inputDeps);
        }
    }

    public enum SteeringType
    {
        Interest,
        Danger
    }
}