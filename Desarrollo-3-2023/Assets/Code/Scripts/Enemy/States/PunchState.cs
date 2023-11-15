using Code.Scripts.Attack;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for punch state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PunchState<T> : BaseState<T>
    {
        private readonly FirePunch leftPunch;
        private readonly FirePunch rightPunch;
        private Vector2 targetPos;

        public bool Ended { get; private set; }

        public PunchState(T id, FirePunch leftPunch, FirePunch rightPunch) : base(id)
        {
            this.leftPunch = leftPunch;
            this.rightPunch = rightPunch;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Punch();
        }

        /// <summary>
        /// Initiate the punch
        /// </summary>
        private void Punch()
        {
            Ended = false;

            if (targetPos.x > 0)
            {
                leftPunch.gameObject.SetActive(true);
                leftPunch.Punch(targetPos);
            }
            else
            {
                rightPunch.gameObject.SetActive(true);
                rightPunch.Punch(targetPos);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!leftPunch.Move && !rightPunch.Move)
                Ended = true;
        }

        /// <summary>
        /// Set the distance to throw the punch to
        /// </summary>
        /// <param name="pos">Position to throw punch to</param>
        public void SetTargetPos(Vector2 pos)
        {
            targetPos = pos;
        }
        
        /// <summary>
        /// Instantly stop punch
        /// </summary>
        public void Stop()
        {
            Ended = true;
            leftPunch.Stop(true);
            rightPunch.Stop(true);
        }
    }
}