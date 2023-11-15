using System;
using System.Collections.Generic;

namespace Patterns.FSM
{
    public class FiniteStateMachine<T>
    {
        private Dictionary<T, BaseState<T>> states = new();
        private BaseState<T> currentState;

        private Dictionary<Type, List<Transition<T>>> _transitions = new Dictionary<Type, List<Transition<T>>>();
        private List<Transition<T>> currentTransitions = new List<Transition<T>>();

        private static List<Transition<T>> EmptyTransitions = new List<Transition<T>>(0);

        private bool initialized;

        public void Init()
        {
            if (!initialized)
                initialized = true;
        }

        public void Update()
        {
            var transition = GetTransition();
 
            if (transition != null)
                SetCurrentState(transition.To);

            if (initialized)
            {
                currentState.OnUpdate();
            }
            else
            {
                throw new System.Exception("FSM not initialized");
            }
        }

        public void FixedUpdate()
        {
            if (initialized)
            {
                currentState.OnFixedUpdate();
            }
            else
            {
                throw new System.Exception("FSM not initialized");
            }
        }

        /// <summary>
        /// Adds newState to fsm's dictionary
        /// </summary>
        /// <param name="newState"></param>
        public void AddState(BaseState<T> newState)
        {
            states.Add(newState.ID, newState);
        }

        /// <summary>
        /// Gets state from fsm's dictionary
        /// </summary>
        /// <param name="stateID"></param>
        /// <returns></returns>
        public BaseState<T> GetState(T stateID)
        {
            if (states.ContainsKey(stateID))
                return states[stateID];
            return null;
        }

        /// <summary>
        /// Gets current state
        /// </summary>
        /// <returns></returns>
        public BaseState<T> GetCurrentState()
        {
            if (currentState != null)
                return currentState;

            return null;
        }

        /// <summary>
        /// Sets current state, running OnExit() methods from previous and OnEnter() from new one
        /// </summary>
        /// <param name="state"></param>
        public void SetCurrentState(BaseState<T> state)
        {
            if (currentState == state) return;

            currentState?.OnExit();

            currentState = state;

            _transitions.TryGetValue(currentState.GetType(), out currentTransitions);
            if (currentTransitions == null)
                currentTransitions = EmptyTransitions;

            currentState?.OnEnter();
        }

        public void AddTransition(BaseState<T> from, BaseState<T> to, Func<bool> condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition<T>>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition<T>(to, condition));
        }

        private Transition<T> GetTransition()
        {
            foreach (var transition in currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }
}

