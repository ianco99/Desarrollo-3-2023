using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "DamagedSettingsSO", menuName = "ScriptableObjects/StateSettings/DamagedSettings")]
    public class DamagedSettings : ScriptableObject
    {
        public TimerSettings timerSettings;
        public float force;
    }
}
