using Code.Scripts.States;
using System.Collections;
using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class JumpStartState<T> : MovementState<T>
    {
        private MonoBehaviour behaviourClass;
        private JumpStartSettings jumpStartSettings;

        private float originalGravScale;
        
        public JumpStartState(T id, MonoBehaviour coroutineContainer, string name, JumpStartSettings startSettings, Transform transform, Rigidbody2D rb) : base(id, name, startSettings.moveSettings, transform, rb)
        {
            jumpStartSettings = startSettings;
            behaviourClass = coroutineContainer;
        }

        public override void OnEnter()
        {
            if (!IsGrounded())
            {
                Exit();
                return;
            }

            base.OnEnter();

            behaviourClass.StartCoroutine(AddForce(transform.up * jumpStartSettings.force, ForceMode2D.Impulse));

            originalGravScale = rb.gravityScale;
            // rb.gravityScale *= 2f;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            rb.gravityScale = originalGravScale;
        }

        public override void OnUpdate()
        {
        }

        private IEnumerator AddForce(Vector3 force, ForceMode2D mode)
        {
            yield return new WaitForFixedUpdate();
            rb.AddForce(force, mode);
        }

        public override void OnFixedUpdate()
        {
            if (rb.velocity.y < 0)
                Exit();
            
            base.OnFixedUpdate();
        }
    }
}
