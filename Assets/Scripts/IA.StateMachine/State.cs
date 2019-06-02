using System.Collections.Generic;

namespace IA.StateMachine.Generic
{
    public class State<T>
    {
        private Dictionary<T, State<T>> transitions = new Dictionary<T, State<T>>();

        public void AddTransition(T key, State<T> nextState)
        {
            transitions[key] = nextState;
        }

        public State<T> GetTransition(T key)
        {
            if (transitions.ContainsKey(key)) return transitions[key];
            else return null;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }

    }
}
