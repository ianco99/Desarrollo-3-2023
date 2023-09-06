using System;
using UnityEngine.InputSystem;

namespace Code.Scripts.Input
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public static event Action<float> onMove;
        public static event Action onAttack;
        public static event Action onBlock;
        public static event Action onJump;

        private void OnMove(InputValue input)
        {
            var axis = input.Get<float>();
        
            onMove?.Invoke(axis);
            print("Move: " + axis);
        }

        private void OnAttack()
        {
            onAttack?.Invoke();
            print("Attack");
        }

        private void OnBlock()
        {
            onBlock?.Invoke();
            print("Block");
        }

        private void OnJump()
        {
            onJump?.Invoke();
            print("Jump");
        }
    }
}
