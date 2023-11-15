using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    /// <summary>
    /// Player detector manager
    /// </summary>
    public class AreaDetector : MonoBehaviour
    {
        [SerializeField] private List<string> detectableTags;
        
        private GameObject detectableInArea;

        private void OnTriggerEnter2D(Collider2D other)
        {
            bool detected = false;
            
            foreach (string detectableTag in detectableTags)
            {
                if (!other.CompareTag(detectableTag)) continue;
                
                detected = true;
            }
            
            if (!detected)
                return;

            detectableInArea = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject != detectableInArea)
                return;
            
            detectableInArea = null;
        }

        /// <summary>
        /// Get if player is in area
        /// </summary>
        /// <returns>true if it is, false if it isn't</returns>
        public bool IsPlayerInArea()
        {
            return detectableInArea;
        }

        /// <summary>
        /// Gives the difference between the detected player and the center of the area
        /// </summary>
        /// <returns>difference in x axis</returns>
        public Vector2 GetPositionDifference()
        {
            if (!detectableInArea)
                return Vector2.zero;

            return detectableInArea.transform.position - transform.position;
        }
    }
}