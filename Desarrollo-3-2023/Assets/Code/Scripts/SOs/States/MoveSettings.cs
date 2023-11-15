using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "MoveSettingsSO", menuName = "ScriptableObjects/StateSettings/MoveSettings")]
    public class MoveSettings : ScriptableObject
    {
        public float speed;
        public float accel;
    }
}