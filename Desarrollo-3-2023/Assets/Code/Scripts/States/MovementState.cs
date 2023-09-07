using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for movement state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MovementState<T> : BaseState<T>
    {
        protected readonly Rigidbody2D rb;
        protected readonly Transform transform;
        private readonly float speed;

        public float dir;

        public MovementState(T id, string name, float speed, Transform transform, Rigidbody2D rb) : base(id, name)
        {
            this.speed = speed;
            this.transform = transform;
            this.rb = rb;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entered Move");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            MoveInDirection(dir);

            if (Mathf.Approximately(dir, 0))
                Exit();
        }

        /// <summary>
        /// Moves current object in given direction
        /// </summary>
        /// <param name="direction"></param>
        protected void MoveInDirection(float direction)
        {
            transform.Translate(Vector3.right * (direction * speed * Time.deltaTime));
        }

        protected bool IsGrounded()
        {
            if (!transform)
                return false;

            var pos = transform.position;
            var scale = transform.localScale;
            var hit = Physics2D.Raycast(pos, Vector2.down, 2f * scale.y, ~(1 << 6 | 1 << 7 | 1 << 8));
            
            Debug.DrawRay(pos, Vector2.down * (1.1f * scale.y));
            
            return hit.collider;
        }
    }
}