using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace AI
{
    public class AgentBase : Behaviour
    {
        public Entity reference;
        public Entity entity;
        public (IAction state, float confidence)[] ideas;
        public IAction[] actions;

        protected List<IReasoner> reasoners = new List<IReasoner>();
        protected List<IEvaluator> evaluators = new List<IEvaluator>();
        protected List<IActuator> actuators = new List<IActuator>();

        private bool shouldThink = true;
        public bool ShouldThink { get => shouldThink; }

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
    }
}