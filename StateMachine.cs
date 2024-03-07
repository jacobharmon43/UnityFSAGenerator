using System;
using System.Collections.Generic;

namespace FSA {
    public class StateMachine {
        public State CurrentState { get; private set; }
        public float TimeInCurrentState { get; private set; }

        private DateTime _timeSinceLastUpdate;
        private readonly Dictionary<string, State> _states = new Dictionary<string, State>();
        private State _startingState;
        private bool _paused = false;
        private bool _initialized = false;

        public StateMachine() {}

        public void AddState(State s) {
            _states.Add(s.Name, s);
        }

        public void SetStartingState(string state) {
            _startingState = _states[state];
        }

        /// <summary>
        /// Also can be used to reset the state machine
        /// </summary>
        public void StartStateMachine() {
            CurrentState = _startingState;
            CurrentState.InitializeState();
            _timeSinceLastUpdate = DateTime.Now;
            _initialized = true;
            _paused = false;
        }

        public void PauseStateMachine() => _paused = true;
        public void UnpauseStateMachine() => _paused = false;

        public void RunStateMachine() {
            // Realistically this should be on the user to do, but its here for convenience and backwards compatibility
            if (!_initialized) { StartStateMachine(); }
            if (_paused) return;
            CurrentState.OnUpdate();
            string tmp = QueryStateChange();   
            if(tmp != CurrentState) {
                ChangeState(tmp);
            }
            DateTime currentTime = DateTime.Now;
            TimeSpan deltaTime = currentTime - _timeSinceLastUpdate;
            TimeInCurrentState = (float)deltaTime.TotalSeconds;
        }

        /// <summary>
        /// Public facing method to change the state whenever, used to override statemachine logic if needed
        /// </summary>
        /// <param name="exitCurrentState"> If true, current states onExit method is called. Otherwise just load the next state </param>
        public void SetState(string stateName, bool exitCurrentState = false) {
            ChangeState(stateName, exitCurrentState);
        }

        private string QueryStateChange() {
            return CurrentState.QueryStateChange();
        }

        private void ChangeState(string nextState, bool exitCurrentState = true) {
            if (exitCurrentState) {
                CurrentState.OnExit();
            }
            CurrentState = _states[nextState];
            CurrentState.InitializeState();
            _timeSinceLastUpdate = DateTime.Now;
            TimeInCurrentState = 0;
        }
    }
}
