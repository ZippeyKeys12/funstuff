namespace AI
{
    public class AIState
    {
        public static readonly AIState NULL = new AIState("NULL");

        protected readonly string name;

        public string Name
            => name;

        public AIState(string name)
        {
            this.name = name;
        }
    }
}