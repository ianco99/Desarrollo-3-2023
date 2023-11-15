using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "ParrySettingsSO", menuName = "ScriptableObjects/StateSettings/ParrySettings")]
    public class ParrySettings : ScriptableObject
    {
        public float duration;
    }
}
