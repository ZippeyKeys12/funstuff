using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Steering {
    public class SteeringBehavior : IActuator {
        public float[] dangerMap, interestMap;

        public string Name
            => "Steering";

        public void Act(AIContext context, List<AIState> states) {
            foreach(var state in states.OfType<SteeringAction>()) {

            }
        }

        public static Vector4 Seek(Vector4 agent, Vector4 target) {
            return target - agent;
        }
    }
}