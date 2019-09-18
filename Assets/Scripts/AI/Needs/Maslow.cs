using System.Collections.Generic;
using System.Linq;

namespace AI.Needs {
    public class MaslowHeirarchy : IReasoner {
        protected readonly List<Need>[] needs;

        public string Name
            => "Maslow";

        public MaslowHeirarchy(List<Need> needsList) {
            needs = new List<Need>[Need.Rungs.Length];

            foreach(var rung in Need.Rungs) {
                needs[(int)rung] = new List<Need>();
            }

            foreach(var need in needsList) {
                needs[(int)need.Rung].Add(need);
            }
        }

        public MaslowHeirarchy(List<Need>[] needs)
            => this.needs = needs;

        public void Add(Need need)
            => needs[(int)need.Rung].Add(need);

        public void Remove(Need need)
            => needs[(int)need.Rung].Remove(need);

        public (AIState, float) Query(AIContext context) {
            foreach(var rung in needs) {
                if(rung.Count != 0 && rung[0] != null) {
                    return (rung[0].State, rung[0].Strength);
                }
            }

            return (AIState.NULL, 0);
        }
    }
}