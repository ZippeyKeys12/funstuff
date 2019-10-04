using System;
using System.Collections.Generic;

// TODO: Inference engine?
namespace AI.Reasoners
{
    public class ProductionSystem : IReasoner
    {
        protected List<(Func<AIContext, bool> precondition, Func<IAction> action)> rules;

        public string Name { get; }

        public ProductionSystem(string name)
            : this(name, new List<(Func<AIContext, bool> precondition, Func<IAction> action)>()) { }

        public ProductionSystem(string name, List<(Func<AIContext, bool> precondition, Func<IAction> action)> rules)
        {
            Name = name;
            this.rules = rules;
        }

        public void AddRule(Func<AIContext, bool> precondition, Func<IAction> action)
            => rules.Add((precondition, action));

        public (IAction state, float confidence) Reason(AIContext context)
        {
            foreach (var rule in rules)
            {
                if (rule.precondition.Invoke(context))
                {
                    return (rule.action.Invoke(), .5f);
                }
            }

            return (ActionUtil.NULL, 0);
        }
    }
}