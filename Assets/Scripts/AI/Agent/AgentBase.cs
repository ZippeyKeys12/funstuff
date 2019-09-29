using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AgentBase : Behaviour
    {
        protected List<ISensor> sensors = new List<ISensor>();
        protected List<IReasoner> reasoners = new List<IReasoner>();
        protected List<IEvaluator> evaluators = new List<IEvaluator>();
        protected List<IActuator> actuators = new List<IActuator>();

        private bool think = true;
        public bool Think { get => think; }

        public void AddSensor<T>(T sensor)
            where T : struct, ISensor
        {
            sensors.Add(sensor);
        }

        public void RemoveSensor<T>()
            where T : ISensor
        {
            sensors.RemoveAll((x) => typeof(T) == x.GetType());
        }

        public ISensor[] GetSensors()
        {
            return sensors.ToArray();
        }

        public void AddReasoner<T>(T reasoner)
            where T : struct, IReasoner
        {
            reasoners.Add(reasoner);
        }

        public void RemoveReasoner<T>()
            where T : IReasoner
        {
            reasoners.RemoveAll((x) => typeof(T) == x.GetType());
        }

        public IReasoner[] GetReasoners()
        {
            return reasoners.ToArray();
        }


        public void AddEvaluator<T>(T evaluator)
            where T : struct, IEvaluator
        {
            evaluators.Add(evaluator);
        }

        public void RemoveEvaluator<T>()
            where T : IEvaluator
        {
            evaluators.RemoveAll((x) => typeof(T) == x.GetType());
        }

        public IEvaluator[] GetEvaluators()
        {
            return evaluators.ToArray();
        }


        public void AddActuator<T>(T actuator)
            where T : struct, IActuator
        {
            actuators.Add(actuator);
        }

        public void RemoveActuator<T>()
            where T : IActuator
        {
            actuators.RemoveAll((x) => typeof(T) == x.GetType());
        }

        public IActuator[] GetActuators()
        {
            return actuators.ToArray();
        }

        public AIContext context;
        public (AIState state, float confidence)[] ideas;
        public AIState[] actions;

        // public AgentBase(ISensor[] sensors, IReasoner[] reasoners, IEvaluator[] evaluators, IActuator[] actuators)
        // {
        //     context = new AIContext(this);

        //     // this.sensors = sensors;
        //     // this.reasoners = reasoners;
        //     // this.evaluators = evaluators;
        //     // this.actuators = actuators;
        // }

        // public void Sense()
        // {
        //     foreach (var sensor in sensors)
        //     {
        //         context += sensor.Sense(this);
        //     }
        // }

        // public void Reason()
        // {
        //     var tempIdeas = new List<(AIState, float)>();
        //     foreach (var reasoner in reasoners)
        //     {
        //         tempIdeas.Add(reasoner.Reason(context));
        //     }

        //     ideas = tempIdeas.ToArray();
        // }

        // public void Evaluate()
        // {
        //     foreach (var evaluator in evaluators)
        //     {
        //         ideas = evaluator.Evaluate(ideas);
        //     }

        //     actions = ideas.Select(x => x.state).ToArray();
        // }

        // public void Act()
        // {
        //     foreach (var actuator in actuators)
        //     {
        //         actuator.Act(actions);
        //     }
        // }
    }
}