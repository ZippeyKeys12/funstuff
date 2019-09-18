using System;
using System.Linq;

namespace AI.Needs {
    public enum NeedsRung {
        // Deficiency
        Physiological,
        Safety,
        Belonging,
        Esteem,

        // Growth
        Cognitive,
        Aesthetic,
        Actualization,
        Transcendence
    }

    public class Need {
        public static readonly NeedsRung[] Rungs
            = Enum.GetValues(typeof(NeedsRung)).Cast<NeedsRung>().ToArray();

        protected readonly NeedsRung rung;
        protected readonly AIState state;
        protected readonly float strength;

        public NeedsRung Rung
            => rung;

        public AIState State
            => state;

        public float Strength
            => strength;

        public Need(NeedsRung rung, AIState state, float strength) {
            this.rung = rung;
            this.state = state;
            this.strength = strength;
        }
    }
}