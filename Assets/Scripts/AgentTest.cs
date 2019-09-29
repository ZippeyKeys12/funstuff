using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using AI;

public class AgentTest : MonoBehaviour
{
    public void Start()
    {
        var entityManager = World.Active.EntityManager;

        var testEntity = entityManager.CreateEntity();

        var agent = new AgentBase();
        agent.AddSensor(new TestSensor());
        agent.AddReasoner(new TestReasoner());
        agent.AddEvaluator(new TestEvaluator());
        agent.AddActuator(new TestActuator());

        entityManager.AddComponentObject(testEntity, agent);
    }

    public struct TestSensor : ISensor
    {
        public string Name
            => "testS";

        public AIContext Sense(Entity entity)
            => new AIContext(entity, new Dictionary<string, object>() { ["testK"] = "testV" });
    }

    public struct TestReasoner : IReasoner
    {
        public string Name
            => "testR";

        public (AIState, float) Reason(AIContext context)
            => (new AIState(context.Get<string>("testK")), 1);
    }

    public struct TestEvaluator : IEvaluator
    {
        public string Name
            => "testE";

        public (AIState state, float confidence)[] Evaluate((AIState state, float confidence)[] ideas)
            => ideas.ToArray();
    }

    public struct TestActuator : IActuator
    {
        public string Name
            => "testA";

        public void Act(Entity entity, AIState[] states)
        {
            if (states.Select(x => x.Name == "testK").Any())
            {
                Debug.Log("testS");
            }
        }
    }
}
