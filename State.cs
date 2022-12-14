using System;
using System.Collections.Generic;

namespace FSA
{
    /* How to use:
     * 
     * The only thing you need to really know, outside of how to construct it
     * is that the transitions list is order-important, so order things in terms
     * of their importance. Or dont allow multiple transitions to be true at once via more complex
     * checks
     * 
     */

    public class State
    {

        public string Name { get; private set; }
        private Action enterState;
        private Action exitState;
        private Action duringState;

        private List<Transition> transitions;

        public State(string name, Action enter, Action during, Action exit)
        {
            this.Name = name;
            this.enterState = enter;
            this.duringState = during;
            this.exitState = exit;
            this.transitions = new List<Transition>();
        }

        public void AssignTransition(State toState, Func<bool> condition)
        {
            Transition t = new Transition(condition, toState);
            transitions.Add(t);
        }

        public int Count(){
            return transitions.Count;
        }

        public void InitializeState()
        {
            enterState?.Invoke();
        }

        public void OnExit()
        {
            exitState?.Invoke();
        }

        public void OnUpdate()
        {
            duringState?.Invoke();
        }

        public State QueryStateChange() //Queries all transitions attached to this state, if any func<bool> result in true, the first one found is returned
        {
            foreach(Transition t in transitions)
            {
                if (t.Check())
                {
                    return t.NextState;
                }
            }
            return this;
        }
        
        public static implicit operator bool(State s) => s != null;
    }
}
