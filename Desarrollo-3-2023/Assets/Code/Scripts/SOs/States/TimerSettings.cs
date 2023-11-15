using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "TimerSettingsSO", menuName = "ScriptableObjects/StateSettings/TimerSettings")]
    public class TimerSettings : ScriptableObject
    {
        public float maxTime;
    }
}
