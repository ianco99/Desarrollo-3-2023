using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Input
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        public static event Action<Vector2> onMove;
        public static event Action onAttackPressed;
        public static event Action onAttackReleased;
        public static event Action onBlockPressed;
        public static event Action onBlockReleased;
        public static event Action onJump;
        public static event Action onPause;
        public static event Action onGodMode;

        private void OnMove(InputValue input)
        {
            Vector2 axis = input.Get<Vector2>();

            onMove?.Invoke(axis);
        }

        private void OnAttack(InputValue input)
        {
            if (input.isPressed)
                onAttackPressed?.Invoke();
            else
                onAttackReleased?.Invoke();
        }

        private void OnBlock(InputValue input)
        {
            if (input.isPressed)
                onBlockPressed?.Invoke();
            else
                onBlockReleased?.Invoke();
        }

        private void OnJump()
        {
            onJump?.Invoke();
        }
        
        private void OnGodMode()
        {
            onGodMode?.Invoke();
        }

        private void OnPause()
        {
            onPause?.Invoke();
        }
    }
}