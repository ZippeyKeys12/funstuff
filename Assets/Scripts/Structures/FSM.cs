using System;
using System.Collections.Generic;
using System.Linq;

namespace Structures
{
    public class FSM<TInput, TState, TStateValue>
        where TState : Enum, IEquatable<TState>
    {
        protected readonly IDictionary<TState, TStateValue> states = new Dictionary<TState, TStateValue>();
        protected readonly IDictionary<TState, Func<TInput, TState>> transitions = new Dictionary<TState, Func<TInput, TState>>();

        protected TStateValue state;

        public FSM((TState a, TStateValue b) defState)
        {
            this.states.Add(defState.a, defState.b);
        }

        public void AddState(TState from, TStateValue to)
        {
            states.Add(from, to);
        }

        public void AddTransition(TState from, Func<TInput, TState> condition)
        {
            transitions.Add(from, condition);
        }

        public TStateValue Evaluate(TInput input)
        {
            return state = states[transitions[states.FirstOrDefault(x => x.Value.Equals(state)).Key](input)];
        }
    }
}