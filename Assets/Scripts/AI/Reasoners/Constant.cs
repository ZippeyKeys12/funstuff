using AI.Agent;
using Unity.Entities;

namespace AI.Reasoners
{
    public class Constant : IReasoner
    {
        protected readonly (IAction, float) constant;

        public string Name { get; }

        public Constant(string name, IAction state, float confidence)
            : this(name, (state, confidence)) { }

        public Constant(string name, (IAction, float) constant)
        {
            Name = name;
            this.constant = constant;
        }

        public (IAction state, float confidence) Reason(Entity entity)
            => this.constant;
    }
}