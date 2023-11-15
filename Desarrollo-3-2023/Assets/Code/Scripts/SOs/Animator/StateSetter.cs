using System.Collections.Generic;

namespace Code.Scripts.SOs.Animator
{
    /// <summary>
    /// Class to manage the state parameter of an animator
    /// </summary>
    /// <typeparam name="T">Type of the key</typeparam>
    public class StateSetter<T>
    {
        private string animatorStateParameter;
        private UnityEngine.Animator animator;
        private Dictionary<T, int> states;
        
        public StateSetter(string animatorStateParameter, UnityEngine.Animator animator)
        {
            this.animatorStateParameter = animatorStateParameter;
            this.animator = animator;
            states = new Dictionary<T, int>();
        }
        
        /// <summary>
        /// Add given state to dictionary
        /// </summary>
        /// <param name="id">Key of the state</param>
        /// <param name="state">Value of the state</param>
        public void AddState(T id, int state)
        {
            states.Add(id, state);
        }

        /// <summary>
        /// Remove given state from dictionary
        /// </summary>
        /// <param name="id">key of the state to remove</param>
        public void RemoveState(T id)
        {
            states.Remove(id);
        }

        /// <summary>
        /// Set the state by key
        /// </summary>
        /// <param name="id">key of the state to set</param>
        public void UpdateAnimator(T id)
        {
            animator.SetInteger(animatorStateParameter, states[id]);
        }
    }
}
