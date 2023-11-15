using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "AttackStartSettingsSO", menuName = "ScriptableObjects/StateSettings/AttackStartSettings")]
    public class AttackStartSettings : ScriptableObject
    {
        public float minTimeOnHold;
        public Color objectiveColor;
    }
}
