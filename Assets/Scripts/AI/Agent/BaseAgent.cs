using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI {
    public class BaseAgent : IAgent {
        protected readonly List<ISensor> sensors;
        protected readonly List<IReasoner> reasoners;
        protected readonly List<IActuator> actuators;
        protected readonly IEvaluator evaluator;

        public BaseAgent()
            : this(new List<ISensor>(), new List<IReasoner>(), new List<IActuator>()) { }

        public BaseAgent(List<ISensor> sensors, List<IReasoner> reasoners, List<IActuator> actuators) {
            this.sensors = sensors;
            this.reasoners = reasoners;
            this.actuators = actuators;
        }

        public void Think() {
            var context = new AIContext(this);

            foreach(var sensor in sensors) {
                context += sensor.Sense(context);
            }

            var ideas = new List<(AIState, float)>();
            foreach(var reasoner in reasoners) {
                var idea = reasoner.Query(context);

                if(idea.Item1 == AIState.NULL || Mathf.Approximately(idea.Item2, 0)) {
                    continue;
                }

                ideas.Add(idea);
            }

            var actions = evaluator.Evaluate(ideas);
            foreach(var actuator in actuators) {
                actuator.Act(context, actions);
            }
        }


        public T GetSensor<T>() where T : ISensor
            => sensors.OfType<T>().First(x => x.GetType() == typeof(T));

        public ISensor GetSensor(string name)
            => sensors.First(x => name.Equals(x.Name));


        public T GetReasoner<T>() where T : IReasoner
            => reasoners.OfType<T>().First(x => x.GetType() == typeof(T));

        public IReasoner GetReasoner(string name)
            => reasoners.First(x => name.Equals(x.Name));


        public T GetActuator<T>() where T : IActuator
            => actuators.OfType<T>().First(x => x.GetType() == typeof(T));

        public IActuator GetActuator(string name)
            => actuators.First(x => name.Equals(x.Name));
    }
}
