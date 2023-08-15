using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using AI;
using AI.Agent;

public class AgentTest : MonoBehaviour
{
    public void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

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

    [UpdateInGroup(typeof(SensorSystemGroup))]
    public class TestSensorSystem : SystemBase
    {
        private EntityCommandBufferSystem entityCommandBufferSystem;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            entityCommandBufferSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref TestSensorComponent testSensorComponent) =>
            {
                ecb.AddComponent(entityInQueryIndex, entity,new TestSensorTag() {text = testSensorComponent.text});
            }).ScheduleParallel();

            entityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }

    public struct TestReasoner : IReasoner
    {
        public string Name
            => "testR";

        public (IAction, float) Reason(Entity entity)
        {
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
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
}
