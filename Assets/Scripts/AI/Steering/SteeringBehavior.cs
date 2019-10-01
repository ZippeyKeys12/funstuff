using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace AI.Actuators.Steering
{
    public class SteeringBehavior : IActuator
    {
        protected float[] contextMap;
        protected Vector3[] basis;
        public string Name { get; }

        public SteeringBehavior(string name)
        {
            Name = name;
        }

        public void Act(Entity entity, AIState[] states)
        {
            float getSpeed(int x)
                => 1 / Mathf.Pow(2, x);

            foreach (var state in states.OfType<SteeringState>())
            {
                var action = state.Action;
                var falloff = state.Falloff;
                var pos = state.Pos;

                SetupBasis(state.ResXY, state.ResYZ, state.ResXZ);

                var idealVec = Vector3.zero;
                if (action == SteeringAction.Seek || action == SteeringAction.Flee)
                {
                    foreach (var target in state.Targets.Select((x, index) => (index, x.pos, x.vel)))
                    {
                        idealVec = Seek(state.Pos, target.pos, getSpeed(target.index));

                        if (action == SteeringAction.Flee)
                        {
                            idealVec *= -1;
                        }
                    }
                }

                else if (action == SteeringAction.Pursue || action == SteeringAction.Evade)
                {
                    foreach (var target in state.Targets.Select((x, index) => (index, x.pos, x.vel)))
                    {
                        idealVec = Pursue(pos, target.pos, target.vel, getSpeed(target.index));

                        if (action == SteeringAction.Evade)
                        {
                            idealVec *= -1;
                        }
                    }
                }

                else if (action == SteeringAction.Arrive)
                {
                    foreach (var target in state.Targets.Select((x, index) => (index, x.pos, x.vel)))
                    {
                        idealVec = Arrive(pos, target.pos, getSpeed(target.index), state.SlowingRadius, state.StoppingRadius);
                    }
                }

                else
                {
                    throw new NotImplementedException(
                        $"{Enum.GetName(typeof(SteeringAction), action)} isn't handled by {typeof(SteeringBehavior).FullName}");
                }

                AddVector(idealVec, falloff);
            }

            // TODO: Affect agent
        }

        protected void SetupBasis(int resXY, int resYZ, int resXZ)
        {
            if (resXY == 0 && resYZ == 0 && resXZ == 0)
            {
                return;
            }

            var tempBasis = new HashSet<Vector3>();

            if (resXY != 0)
            {
                for (var i = 0f; i < 2 * Mathf.PI; i += Mathf.PI / (2 * resXY))
                {
                    tempBasis.Add(new Vector3(Mathf.Cos(i), Mathf.Sin(i), 0));
                }
            }

            if (resYZ != 0)
            {
                for (var i = 0f; i < 2 * Mathf.PI; i += Mathf.PI / (2 * resXY))
                {
                    tempBasis.Add(new Vector3(0, Mathf.Sin(i), Mathf.Cos(i)));
                }
            }

            if (resXZ != 0)
            {
                for (var i = 0f; i < 2 * Mathf.PI; i += Mathf.PI / (2 * resXY))
                {
                    tempBasis.Add(new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i)));
                }
            }

            basis = tempBasis.ToArray();
            contextMap = new float[basis.Length];
        }

        protected void AddVector(Vector3 finalVec, int falloff)
        {
            var closestDir = basis.Select((x, index) => (index, value: Vector3.Dot(finalVec, x)))
                                  .Aggregate((a, b) => a.value > b.value ? a : b)
                                  .index;

            int wraparoundIndex(int index)
                => index < 0 ? wraparoundIndex(contextMap.Length + index) : index % contextMap.Length;

            for (int i = 0; i <= falloff; i++)
            {
                var top = wraparoundIndex(closestDir + falloff);
                var bottom = wraparoundIndex(closestDir - falloff);
                contextMap[top] = Vector3.Dot(finalVec, basis[top]) / Mathf.Pow(2, i);
                contextMap[bottom] = Vector3.Dot(finalVec, basis[bottom]) / Mathf.Pow(2, i);
            }
        }

        public static Vector3 Seek(Vector3 agentPos, Vector3 target, float maxSpeed)
            => maxSpeed * (target - agentPos).normalized;

        public static Vector3 Flee(Vector3 agentPos, Vector3 target, float maxSpeed)
            => -Seek(agentPos, target, maxSpeed);

        public static Vector3 Pursue(Vector3 agentPos, Vector3 target, Vector3 vel, float maxSpeed)
            => Seek(agentPos, target + vel, maxSpeed);

        public static Vector3 Evade(Vector3 agentPos, Vector3 target, Vector3 vel, float maxSpeed)
            => -Pursue(agentPos, target, vel, maxSpeed);

        public static Vector3 Arrive(Vector3 agentPos, Vector3 target, float maxSpeed, float slowingRadius, float stoppingRadius = 0)
        {
            var diff = target - agentPos;

            return maxSpeed * Mathf.InverseLerp(stoppingRadius, slowingRadius, diff.magnitude) * diff.normalized;
        }
    }
}