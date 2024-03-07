using System;
using System.Collections.Generic;

namespace FSA {
    public class State {
        public string Name { get; private set; }
        private readonly Action _onEnter;
        private readonly Action _onRun;
        private readonly Action _onExit;
        private readonly List<Transition> _transitions = new List<Transition>();

        public State(string name, Action enter, Action during, Action exit) {
            Name = name;
            _onEnter = enter;
            _onRun = during;
            _onExit = exit;
            _transitions = new List<Transition>();
        }

        public void AssignTransition(Transition t) {
            _transitions.Add(t);
        }

        public void InitializeState() {
            _onEnter?.Invoke();
        }

        public void OnExit() {
            _onExit?.Invoke();
        }

        public void OnUpdate() {
            _onRun?.Invoke();
        }

        public string QueryStateChange() {
            foreach(Transition t in _transitions) {
                if (t.Check()) {
                    return t.NextState;
                }
            }
            return Name;
        }
        
        public static implicit operator bool(State s) => s != null;
        public static bool operator ==(string s, State state) => s == state.Name;
        public static bool operator !=(string s, State state) => s != state.Name;
        public static bool operator ==(State state, string s) => state.Name == s;
        public static bool operator !=(State state, string s) => state.Name != s;
    }
}
