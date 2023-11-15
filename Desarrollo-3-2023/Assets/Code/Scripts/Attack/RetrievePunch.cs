using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Component to control the retrieving of the punch
    /// </summary>
    public class RetrievePunch : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        
        private Vector3 startPos;
        
        public bool Move { get; private set; }

        private void Start()
        {
            startPos = transform.localPosition;
        }

        private void Update()
        {
            if (!Move) return;
            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, speed * Time.deltaTime);
            
            CheckRetrieveFinished();
        }

        /// <summary>
        /// Initiate the retrieval
        /// </summary>
        public void Retrieve()
        {
            Move = true;
        }

        /// <summary>
        /// Stop retrieving if hand reached its position
        /// </summary>
        private void CheckRetrieveFinished()
        {
            if (transform.localPosition == startPos)
                Move = false;
        }
        
        /// <summary>
        /// Return if the hand is in its position
        /// </summary>
        /// <returns>true if it is</returns>
        public bool InPos()
        {
            return transform.localPosition == startPos;
        }
        
        public void Reset()
        {
            Move = false;
            transform.localPosition = startPos;
        }
    }
}
