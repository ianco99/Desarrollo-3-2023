using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "JumpStartSettingsSO", menuName = "ScriptableObjects/StateSettings/JumpStartSettings")]
    public class JumpStartSettings : ScriptableObject
    {
        [FormerlySerializedAs("jumpForce")] public float force;
        public MoveSettings moveSettings;
    }
}
