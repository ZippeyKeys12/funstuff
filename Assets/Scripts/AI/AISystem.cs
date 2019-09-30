using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace AI
{
    public class AISystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            var allEntities = GameManager.GetAllEntities();
            var agents = allEntities.Where(e => EntityManager.HasComponent<AgentBase>(e))
                                    .Select(e => (e, a: EntityManager.GetComponentObject<AgentBase>(e)))
                                    .Where(t => t.a.ShouldThink)
                                    .ToArray();
            allEntities.Dispose();

            foreach (var pair in agents)
            {
                pair.a.context = new AIContext(pair.e);
                pair.a.context["owner"] = pair.e;
                foreach (var sensor in pair.a.GetSensors())
                {
                    pair.a.context += sensor.Sense(pair.e);
                }
            }

            foreach (var pair in agents)
            {
                var ideas = new List<(AIState state, float confidence)>();
                foreach (var reasoner in pair.a.GetReasoners())
                {
                    ideas.Add(reasoner.Reason(pair.a.context));
                }

                var plans = ideas.ToArray();
                foreach (var evaluator in pair.a.GetEvaluators())
                {
                    plans = evaluator.Evaluate(plans);
                }

                var actions = plans.Select(x => x.state).ToArray();
                foreach (var actuactor in pair.a.GetActuators())
                {
                    actuactor.Act(pair.e, actions);
                }
            }
        }
    }
}
