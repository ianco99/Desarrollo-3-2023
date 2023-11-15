using Code.Scripts.States;
using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for jump end state
    /// </summary>
    public class JumpEndState<T> : MovementState<T>
    {
        private JumpEndSettings jumpEndSettings;

        private float originalGravScale;

        public JumpEndState(T id, string name, JumpEndSettings settings, Transform transform, Rigidbody2D rb) : base(id, name, settings.moveSettings, transform, rb)
        {
            jumpEndSettings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            originalGravScale = rb.gravityScale;
            rb.gravityScale *= jumpEndSettings.gravMultiplier;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            rb.gravityScale = originalGravScale;
        }

        public override void OnUpdate()
        {
            if (IsGrounded())
                Exit();
        }
    }
}
