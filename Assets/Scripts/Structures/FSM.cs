using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure
{
    public class FSM<I, T, K>
        where T : Enum, IEquatable<T>
    {
        protected readonly IDictionary<T, K> states = new Dictionary<T, K>();
        protected readonly IDictionary<T, Func<I, T>> transitions = new Dictionary<T, Func<I, T>>();

        protected K state;

        public FSM((T a, K b) defState)
        {
            this.states.Add(defState.a, defState.b);
        }

        public void AddState(T from, K to)
        {
            states.Add(from, to);
        }

        public void AddTransition(T from, Func<I, T> condition)
        {
            transitions.Add(from, condition);
        }

        public K Evaluate(I input)
        {
            return state = states[transitions[states.FirstOrDefault(x => x.Value.Equals(state)).Key](input)];
        }
    }
}