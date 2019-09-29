using System.Collections.Generic;

namespace AI.Needs
{
    public class MaslowHeirarchy : IReasoner
    {
        protected readonly List<Need>[] needs;

        public string Name { get; }

        public MaslowHeirarchy(string name, List<Need> needsList)
        {
            Name = name;

            needs = new List<Need>[Need.Rungs.Length];

            foreach (var rung in Need.Rungs)
            {
                needs[(int)rung] = new List<Need>();
            }

            foreach (var need in needsList)
            {
                needs[(int)need.Rung].Add(need);
            }
        }

        public MaslowHeirarchy(string name, List<Need>[] needs)
        {
            Name = name;
            this.needs = needs;
        }

        public void Add(Need need)
            => needs[(int)need.Rung].Add(need);

        public void Remove(Need need)
            => needs[(int)need.Rung].Remove(need);

        public (AIState, float) Reason(AIContext context)
        {
            foreach (var rung in needs)
            {
                if (rung.Count != 0 && rung[0] != null)
                {
                    return (rung[0].State, rung[0].Strength);
                }
            }

            return (AIState.NULL, 0);
        }
    }
}