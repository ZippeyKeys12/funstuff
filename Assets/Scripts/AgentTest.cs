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

        for (var i = 0; i < 50; i++)
        {
            var testEntity = entityManager.CreateEntity();

            var agent = new AgentBase();

            for (var j = 0; j < 50; j++)
            {
                agent.AddSensor(new TestSensor());
            }

            for (var j = 0; j < 50; j++)
            {
                agent.AddReasoner(new TestReasoner());
            }

            for (var j = 0; j < 50; j++)
            {
                agent.AddEvaluator(new TestEvaluator());
            }

            for (var j = 0; j < 50; j++)
            {
                agent.AddActuator(new TestActuator());
            }

            entityManager.AddComponentObject(testEntity, agent);
        }
    }

    public struct TestAction : IAction
    {
        public string Name { get; }

        public TestAction(string name)
        {
            Name = name;
        }
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

        public (IAction, float) Reason(AIContext context)
            => (new TestAction(context.Get<string>("testK")), 1);
    }

    public struct TestEvaluator : IEvaluator
    {
        public string Name
            => "testE";

        public (IAction state, float confidence)[] Evaluate((IAction state, float confidence)[] ideas)
            => ideas.ToArray();
    }

    public struct TestActuator : IActuator
    {
        public string Name
            => "testA";

        public void Act(Entity entity, IAction[] states)
        {
            if (states.Select(x => ((TestAction)x).Name == "testK").Any())
            {
                // Debug.Log("testS");
            }
        }
    }
}
