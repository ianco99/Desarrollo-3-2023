using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName ="AlertSettingsSO", menuName = "ScriptableObjects/StateSettings/AlertSettings")]
    public class AlertSettings : ScriptableObject, IAlertSettings
    {
        public MoveSettings moveSettings;
        public float alertAttackDistance;

        [Header("GroundCheck")]
        public float groundCheckDistance;
        public LayerMask groundLayer;
    }
}
