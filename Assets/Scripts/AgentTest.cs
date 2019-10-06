using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
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

            entityManager.AddComponentData(testEntity, new TestSensorComponent() { text = new NativeString512("testK") });
            agent.AddReasoner(new TestReasoner());

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

    public struct TestSensorComponent : ISensorComponent
    {
        public NativeString512 text;
    }

    public struct TestSensorTag : IComponentData
    {
        public NativeString512 text;
    }

    // [UpdateBefore(typeof(AISystem))]
    // [UpdateInGroup(typeof(SimulationSystemGroup))]

    [UpdateInGroup(typeof(SensorSystemGroup))]
    public class TestSensorSystem : JobComponentSystem
    {
        // [BurstCompile]
        struct TestSensorJob : IJobForEachWithEntity<TestSensorComponent>
        {
            public EntityCommandBuffer.Concurrent entityCommandBuffer;

            public void Execute(Entity entity, int index, [ReadOnly] ref TestSensorComponent testSensorComponent)
            {
                entityCommandBuffer.AddComponent(index, entity, new TestSensorTag() { text = testSensorComponent.text });
            }
        }

        private EntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnStartRunning()
        {
            entityCommandBufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
            base.OnStartRunning();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobHandle = new TestSensorJob()
            {
                entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDeps);

            entityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }

    public struct TestReasoner : IReasoner
    {
        public string Name
            => "testR";

        public (IAction, float) Reason(Entity entity)
        {
            var manager = World.Active.EntityManager;
            if (manager.HasComponent<TestSensorTag>(entity))
            {
                var comp = manager.GetComponentData<TestSensorTag>(entity);
                manager.RemoveComponent<TestSensorTag>(entity);

                return (new TestAction(comp.text.ToString()), 1);
            }

            return (ActionUtil.NULL, 0);
        }
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
            if (states.Where(x => x != ActionUtil.NULL).Select(x => ((TestAction)x).Name == "testK").Any())
            {
                print("testS");
            }
        }
    }
}
