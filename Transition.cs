using System;

namespace FSA
{
    /* How to use:
     * 
     * Reading the code would suffice, but its just a class that holds a boolean check
     * and a desired next state if that check is true
     * 
     */

    public class Transition
    {
        public State NextState { get; private set; }
        private Func<bool> condition;

        public Transition(Func<bool> condition, State nextState)
        {
            this.condition = condition;
            this.NextState = nextState;
        }

        public bool Check()
        {
            if(condition == null) return false;
            return condition.Invoke();
        }
    }
}
