using System;
using System.Collections.Generic;
using Unity.Entities;

// TODO: Inference engine?
namespace AI.Reasoners
{
    public class ProductionSystem : IReasoner
    {
        protected List<(Func<Entity, bool> precondition, Func<IAction> action)> rules;

        public string Name { get; }

        public ProductionSystem(string name)
            : this(name, new List<(Func<Entity, bool> precondition, Func<IAction> action)>()) { }

        public ProductionSystem(string name, List<(Func<Entity, bool> precondition, Func<IAction> action)> rules)
        {
            Name = name;
            this.rules = rules;
        }

        public void AddRule(Func<Entity, bool> precondition, Func<IAction> action)
            => rules.Add((precondition, action));

        public (IAction state, float confidence) Reason(Entity entity)
        {
            foreach (var rule in rules)
            {
                if (rule.precondition.Invoke(entity))
                {
                    return (rule.action.Invoke(), .5f);
                }
            }

            return (ActionUtil.NULL, 0);
        }
    }
}