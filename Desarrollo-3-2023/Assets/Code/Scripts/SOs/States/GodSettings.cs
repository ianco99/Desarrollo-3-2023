using UnityEngine;

namespace Code.SOs.States
{
    [CreateAssetMenu(fileName = "GodSettingsSO", menuName = "ScriptableObjects/StateSettings/GodSettings")]
    public class GodSettings : ScriptableObject
    {
        public MoveSettings moveSettings;
    }
}