using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace AI
{
    public class AISystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            var allEntities = GameManager.GetAllEntities();
            foreach (var entity in allEntities)
            {
                if (!EntityManager.HasComponent<AgentBase>(entity))
                {
                    continue;
                }

                var agent = EntityManager.GetComponentObject<AgentBase>(entity);

                if (!agent.Think)
                {
                    continue;
                }

                agent.context = new AIContext(entity);
                agent.context["owner"] = entity;
                foreach (var sensor in agent.GetSensors())
                {
                    agent.context += sensor.Sense(entity);
                }
            }

            foreach (var entity in allEntities)
            {
                if (!EntityManager.HasComponent<AgentBase>(entity))
                {
                    continue;
                }

                var agent = EntityManager.GetComponentObject<AgentBase>(entity);

                if (!agent.Think)
                {
                    continue;
                }

                var ideas = new List<(AIState state, float confidence)>();
                foreach (var reasoner in agent.GetReasoners())
                {
                    ideas.Add(reasoner.Reason(agent.context));
                }
                agent.ideas = ideas.ToArray();
            }

            foreach (var entity in allEntities)
            {
                if (!EntityManager.HasComponent<AgentBase>(entity))
                {
                    continue;
                }

                var agent = EntityManager.GetComponentObject<AgentBase>(entity);

                if (!agent.Think)
                {
                    continue;
                }

                foreach (var evaluator in agent.GetEvaluators())
                {
                    agent.ideas = evaluator.Evaluate(agent.ideas);
                }
            }

            foreach (var entity in allEntities)
            {
                if (!EntityManager.HasComponent<AgentBase>(entity))
                {
                    continue;
                }

                var agent = EntityManager.GetComponentObject<AgentBase>(entity);

                if (!agent.Think)
                {
                    continue;
                }

                var actions = agent.ideas.Select(x => x.state).ToArray();
                foreach (var actuactor in agent.GetActuators())
                {
                    actuactor.Act(entity, actions);
                }
            }
            allEntities.Dispose();
        }
    }
}
