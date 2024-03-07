using System.Collections.Generic;
using System;

namespace FSA
{

    /* How to use:
     * 
     * Start with a StateMachine x = new StateMachineBuilder()
     * Define a state using .WithState( stateName : string)
     * Define the states actions using .WithOnEnter, .WithOnRun, .WithOnExit
     * Define the states transition(s) using .WithTransition(nextState : string, Func<bool>)
     * Defining transitions for states that don't exist will not error, but will just produce nothing
     * Continue this pattern, define the next state with .WithState... and do this until there are no more states.
     * call .Build()
     * EXAMPLE:
     *             StateMachine x = new StateMachineBuilder()
     *                          .WithState("X")
     *                          .WithOnEnter( () => { foo(); } )
     *                          .WithTransition("Y", () => { return bar(); } )
     *                          .WithState("Y")
     *                          .WithOnEnter( () => { foobar(); } )
     *                          .Build()
     */



    public class StateMachineBuilder
    {
        private readonly List<string> _states = new();
        private readonly Dictionary<string, List<Action>> _onEnter = new();
        private readonly Dictionary<string, List<Action>> _onExit = new();
        private readonly Dictionary<string, List<Action>> _onRun = new();
        private readonly Dictionary<string, List<(string, Func<bool>)>> _transitions = new();
        private string _currentState;

        public StateMachineBuilder WithState(string name) {
            if (_states.Contains(name)) throw new ArgumentException($"State {name} already exists.");
            _states.Add(name);
            _currentState = name;
            return this;
        }

        public StateMachineBuilder WithOnEnter(Action onEnter) {
            TryAddToDictionary(_onEnter, _currentState, onEnter);
            return this;
        }

        public StateMachineBuilder WithOnRun(Action onRun) {
            TryAddToDictionary(_onRun, _currentState, onRun);
            return this;
        }
        
        public StateMachineBuilder WithOnExit(Action onExit) {
            TryAddToDictionary(_onExit, _currentState, onExit);
            return this;
        }

        public StateMachineBuilder WithTransition(string nextState, Func<bool> condition) {
            TryAddToDictionary(_transitions, _currentState, (nextState, condition));
            return this;
        }

        public StateMachine Build() {
            StateMachine sm = new StateMachine();
            foreach (string state in _states) {
                State s = new(state, ConsolidateActions(_onEnter.GetValueOrDefault(state)), ConsolidateActions(_onRun.GetValueOrDefault(state)), ConsolidateActions(_onExit.GetValueOrDefault(state)));
                if (_transitions.ContainsKey(state)) {
                    foreach ((string, Func<bool>) transition in _transitions[state]) {
                        s.AssignTransition(new Transition(transition.Item1, transition.Item2));
                    }
                }
                sm.AddState(s);
            }
            sm.SetStartingState(_states[0]);
            return sm;
        }

        Action ConsolidateActions(List<Action> l) {
            Action allActions = null;
            if (l == null) return allActions;
            foreach (Action action in l) {
                allActions += action;
            }
            return allActions;
        }

        private void TryAddToDictionary<T>(Dictionary<string, List<T>> dictionary, string key, T value) {
            if (dictionary.ContainsKey(key)) {
                dictionary[key].Add(value);
            } else {
                dictionary.Add(key, new List<T>() { value });
            }
        }
    }
}
