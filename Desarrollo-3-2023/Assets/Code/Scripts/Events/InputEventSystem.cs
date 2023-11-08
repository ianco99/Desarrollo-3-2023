using Code.Scripts.Input;
using kuznickiEventChannel;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class InputEventSystem : MonoBehaviourSingleton<InputEventSystem>
    {
        [SerializeField] private Vector2EventChannel onMoveChannel;
        [SerializeField] private VoidEventChannel onAttackChannel;
        [SerializeField] private VoidEventChannel onBlockChannel;
        [SerializeField] private VoidEventChannel onJumpChannel;
        [SerializeField] private VoidEventChannel onPauseChannel;

        public Vector2EventChannel OnMoveChannel { get => onMoveChannel; private set => onMoveChannel = value; }
        public VoidEventChannel OnAttackChannel { get => onAttackChannel; private set => onAttackChannel = value; } 
        public VoidEventChannel OnBlockChannel { get => onBlockChannel; private set => onBlockChannel = value; }
        public VoidEventChannel OnJumpChannel { get => onJumpChannel; private set => onJumpChannel = value; }


        private void Start()
        {
            InputManager.onMove += MoveCharacters;
            InputManager.onAttackPressed += AttackPressedAttackers;
            InputManager.onBlockPressed += BlockBlockers;
            InputManager.onJump += JumpJumpers;
            InputManager.onPause += TogglePause;
        }

        private void MoveCharacters(Vector2 axis)
        {
            onMoveChannel.RaiseEvent(axis);
        }

        private void AttackPressedAttackers()
        {
            onAttackChannel.RaiseEvent();
        }
    
        private void BlockBlockers()
        {
            onBlockChannel.RaiseEvent();
        }

        private void JumpJumpers()
        {
            onJumpChannel.RaiseEvent();
        }

        private void TogglePause()
        {
            onPauseChannel.RaiseEvent();
        }
    }
}
