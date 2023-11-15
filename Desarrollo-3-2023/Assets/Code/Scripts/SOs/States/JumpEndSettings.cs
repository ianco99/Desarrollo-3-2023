using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "JumpEndSettingsSO", menuName = "ScriptableObjects/StateSettings/JumpEndSettings")]
    public class JumpEndSettings : ScriptableObject
    {
        public float gravMultiplier;
        public MoveSettings moveSettings;
    }
}
