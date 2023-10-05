using System;
using Code.Scripts.Input;
using Code.Scripts.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Player
{
    [Serializable]
    public enum PlayerStates
    {
        Idle,
        Move,
        JumpStart,
        JumpEnd,
        Attack,
        Block,
        Parry,
    }

    public class PlayerController : Character
    {
        [Header("Player:")] [SerializeField] private PlayerStates startState = PlayerStates.Idle;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float parryDuration = 1f;
        [SerializeField] private float friction = 5f;
        [SerializeField] private GameObject hit;
        [SerializeField] private GameObject parryCapsule;
        [SerializeField] private GameObject blockCapsule;
        [SerializeField] private Damageable damageable;
        [SerializeField] private PhysicsMaterial2D physMat;

        // States
        private MovementState<PlayerStates> movementState;
        private IdleState<PlayerStates> idleState;
        private AttackState<PlayerStates> attackState;
        private ParryState<PlayerStates> parryState;
        private BlockState<PlayerStates> blockState;
        private JumpStartState<PlayerStates> jumpStartState;
        private JumpEndState<PlayerStates> jumpEndState;

        private FiniteStateMachine<PlayerStates> fsm;

        private void Awake()
        {
            Transform trans = transform;

            movementState =
                new MovementState<PlayerStates>(PlayerStates.Move, "MovementState", speed, acceleration, trans, rb);
            idleState = new IdleState<PlayerStates>(PlayerStates.Idle, "IdleState");
            attackState = new AttackState<PlayerStates>(PlayerStates.Attack, "AttackState", hit);
            parryState = new ParryState<PlayerStates>(PlayerStates.Parry, "ParryState", damageable, parryDuration);
            blockState = new BlockState<PlayerStates>(PlayerStates.Block, "BlockState", damageable);
            jumpStartState = new JumpStartState<PlayerStates>(PlayerStates.JumpStart, this, "JumpStartState", speed,
                acceleration, trans, rb, jumpForce);
            jumpEndState =
                new JumpEndState<PlayerStates>(PlayerStates.JumpEnd, "JumpEndState", speed, acceleration, trans, rb);

            fsm = new FiniteStateMachine<PlayerStates>();

            fsm.AddState(movementState);
            fsm.AddState(idleState);
            fsm.AddState(attackState);
            fsm.AddState(parryState);
            fsm.AddState(blockState);
            fsm.AddState(jumpStartState);
            fsm.AddState(jumpEndState);

            fsm.SetCurrentState(fsm.GetState(startState));

            fsm.Init();
        }

        private void OnEnable()
        {
            InputManager.onMove += CheckMoving;
            InputManager.onAttack += CheckAttack;
            InputManager.onBlockPressed += CheckParry;
            InputManager.onBlockReleased += CheckBlock;
            InputManager.onJump += CheckJumpStart;
        }

        private void OnDisable()
        {
            InputManager.onMove -= CheckMoving;
            InputManager.onAttack -= CheckAttack;
            InputManager.onBlockPressed -= CheckParry;
            InputManager.onBlockReleased -= CheckBlock;
            InputManager.onJump -= CheckJumpStart;
        }

        private void Update()
        {
            CheckPlayerState();
            CheckJumpEnd();
            CheckRotation();
            ChangeFeetFriction();

            fsm.Update();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        /// <summary>
        /// Sets player's current state
        /// </summary>
        private void CheckPlayerState()
        {
            blockCapsule.SetActive(false);
            parryCapsule.SetActive(false);

            switch (fsm.GetCurrentState().ID)
            {
                case PlayerStates.Idle:
                    IdleTransitions();
                    break;

                case PlayerStates.Move:
                    MoveTransitions();
                    break;

                case PlayerStates.JumpStart:
                    JumpStartTransitions();
                    break;

                case PlayerStates.Attack:
                    AttackTransitions();
                    break;

                case PlayerStates.Block:
                    blockCapsule.SetActive(true);
                    BlockTransitions();
                    break;

                case PlayerStates.Parry:
                    parryCapsule.SetActive(true);
                    ParryTransitions();
                    break;

                case PlayerStates.JumpEnd:
                    JumpEndTransitions();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Control player facing direction
        /// </summary>
        private void CheckRotation()
        {
            switch (movementState.dir)
            {
                case > 0:
                {
                    if (!facingRight)
                        Flip();

                    break;
                }
                case < 0:
                {
                    if (facingRight)
                        Flip();

                    break;
                }
            }
        }

        /// <summary>
        /// Check if player is grounded and update the feet friction
        /// </summary>
        private void ChangeFeetFriction()
        {
            if (movementState.IsGrounded())
                physMat.friction = friction;
            else
                physMat.friction = 0f;
        }

        #region State activations

        /// <summary>
        /// Handle to move transition
        /// </summary>
        /// <param name="input"></param>
        private void CheckMoving(float input)
        {
            if (input != 0)
                movementState.Enter();

            movementState.dir = input;
            jumpStartState.dir = input;
            jumpEndState.dir = input;
        }

        /// <summary>
        /// Handle to attack transition
        /// </summary>
        private void CheckAttack()
        {
            if (fsm.GetCurrentState().ID == PlayerStates.Idle || fsm.GetCurrentState().ID == PlayerStates.Move)
                attackState.Enter();
        }

        /// <summary>
        /// Handle to parry transition
        /// </summary>
        private void CheckParry()
        {
            if (fsm.GetCurrentState().ID != PlayerStates.Idle && fsm.GetCurrentState().ID != PlayerStates.Move) return;

            parryState.Enter();
            blockState.Enter();
        }

        /// <summary>
        /// Handle to block transition
        /// </summary>
        private void CheckBlock()
        {
            blockState.Exit();
        }

        /// <summary>
        /// Handle to jump start transition
        /// </summary>
        private void CheckJumpStart()
        {
            jumpStartState.Enter();
        }

        /// <summary>
        /// Handle to jump end transition
        /// </summary>
        private void CheckJumpEnd()
        {
            if (rb.velocity.y < 0)
                jumpEndState.Enter();
        }

        #endregion

        #region Transitions

        /// <summary>
        /// Idle state transitions manager
        /// </summary>
        private void IdleTransitions()
        {
            if (movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Move));
            else if (attackState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Attack));
            else if (parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Parry));
            else if (jumpStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.JumpStart));
        }

        /// <summary>
        /// Move state transitions manager
        /// </summary>
        private void MoveTransitions()
        {
            if (!movementState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
            else if (jumpStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.JumpStart));
            else if (attackState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Attack));
            else if (parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Parry));
        }

        /// <summary>
        /// Attack state transitions manager
        /// </summary>
        private void AttackTransitions()
        {
            if (!attackState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        /// <summary>
        /// Parry state transitions manager
        /// </summary>
        private void ParryTransitions()
        {
            if (!parryState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Block));
        }

        /// <summary>
        /// Block state transitions manager
        /// </summary>
        private void BlockTransitions()
        {
            if (!blockState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        /// <summary>
        /// Jump start state transitions manager
        /// </summary>
        private void JumpStartTransitions()
        {
            if (!jumpStartState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.JumpEnd));
        }

        /// <summary>
        /// Jump end state transitions manager
        /// </summary>
        private void JumpEndTransitions()
        {
            if (!jumpEndState.Active)
                fsm.SetCurrentState(fsm.GetState(PlayerStates.Idle));
        }

        #endregion
    }
}