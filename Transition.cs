using System;

namespace FSA
{
    /// <summary>
    /// Stores a condition, and a state name to transition to
    /// </summary>
    public class Transition
    {
        public string NextState { get; private set; }
        private readonly Func<bool> _condition;

        public Transition(string nextState, Func<bool> condition) {
            _condition = condition;
            NextState = nextState;
        }

        public bool Check() {
            if(_condition == null) return false;
            return _condition.Invoke();
        }
    }
}
