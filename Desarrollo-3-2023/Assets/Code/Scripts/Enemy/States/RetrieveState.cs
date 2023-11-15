using Code.Scripts.Attack;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for retrieve state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RetrieveState<T> : BaseState<T>
    {
        private RetrievePunch leftPunch;
        private RetrievePunch rightPunch;
        
        public bool Ended { get; private set; }
        
        public RetrieveState(T id, RetrievePunch leftPunch, RetrievePunch rightPunch) : base(id)
        {
            this.leftPunch = leftPunch;
            this.rightPunch = rightPunch;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Ended = false;

            if (!leftPunch.InPos())
                leftPunch.Retrieve();
            if (!rightPunch.InPos())
                rightPunch.Retrieve();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!leftPunch.Move && !rightPunch.Move)
                Ended = true;
        }
        
        /// <summary>
        /// Instantly retrieve punches
        /// </summary>
        public void Reset()
        {
            Ended = true;
            
            leftPunch.Reset();
            rightPunch.Reset();
        }
    }
}
