namespace AI
{
    public class Constant : IReasoner
    {
        protected readonly (AIState, float) constant;

        public string Name { get; }

        public Constant(string name, AIState state, float confidence)
            : this(name, (state, confidence)) { }

        public Constant(string name, (AIState, float) constant)
        {
            Name = name;
            this.constant = constant;
        }

        public (AIState state, float confidence) Reason(AIContext context)
            => this.constant;
    }
}