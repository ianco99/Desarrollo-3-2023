using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for idle state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IdleState<T> : BaseState<T>
    {
        public IdleState(T id) : base(id)
        {
        }

        public IdleState(T id, string name) : base(id, name)
        {
        }
    }
}
