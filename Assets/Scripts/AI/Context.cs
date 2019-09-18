using System.Collections.Generic;
using System.Linq;

namespace AI {
    public class AIContext {
        protected readonly IAgent owner;
        protected readonly Dictionary<string, object> pairs;

        public IAgent Owner
            => owner;

        public AIContext(IAgent owner)
            : this(owner, new Dictionary<string, object>()) { }

        public AIContext(IAgent owner, Dictionary<string, object> pairs) {
            this.owner = owner;
            this.pairs = pairs;
        }

        public object this[string key]
            => pairs[key];

        public static AIContext operator +(AIContext a, AIContext b)
            => new AIContext(b.owner, new Dictionary<string, object>(
                a.pairs.Concat(b.pairs)
                .ToLookup(p => p.Key, p => p.Value)
                .ToDictionary(g => g.Key, g => g.Last())));
    }
}