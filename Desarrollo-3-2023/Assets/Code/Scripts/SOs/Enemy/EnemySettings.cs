using Code.SOs.States;
using kuznickiEventChannel;
using UnityEngine;

namespace Code.SOs.Enemy
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings")]

    public class EnemySettings : ScriptableObject
    {

        [Header("Patrol")]
        public PatrolSettings patrolSettings;

        [Header("Alert")]
        public AlertSettings alertSettings;
        
        [Header("Attack")]
        public AttackStartSettings attackStartSettings;

        [Header("Damaged")]
        public DamagedSettings damagedSettings;

        [Header("Block")]
        public KnockbackBlockSettings blockSettings;

        [Header("Parry")]
        public ParrySettings parrySettings;

        [Header("Field of View")]
        public float viewRadius;
        public float viewAngle;

        [Header("Suspect meter")]
        public float suspectMeterMinimum = 0.0f;
        public float alertValue = 40.0f;
        public float suspectMeterMaximum = 100.0f;
        
        [Header("Death event")]
        public VoidEventChannel deathEvent;
    }
}
