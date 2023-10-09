using System;
using Code.FOV;
using Code.Scripts.Abstracts.Character;
using Code.Scripts.Attack;
using Code.Scripts.States;
using Code.SOs.Enemy;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Enemy
{
    [Serializable]
    public enum EnemyStates
    {
        Patrol,
        Alert,
        Attack,
        Block,
        Parry,
        Damaged,
        Fall
    }

    public class EnemyController : Character
    {
        [SerializeField] private EnemyStates startingState;

        [SerializeField] private HitsManager hitsManager;
        [SerializeField] private EnemySettings settings;
        [SerializeField] private FieldOfView fov;
        [SerializeField] private SpriteMask suspectMeterMask;
        [SerializeField] private SpriteRenderer suspectMeterSprite;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Damageable damageable;

        [SerializeField] private float suspectMeter;
        [SerializeField] private float suspectUnit = 0.5f;
        [SerializeField] private float hitDistance = 5f;

        private Transform detectedPlayer;
        private bool turnedAggro;

        private FiniteStateMachine<EnemyStates> fsm;
        private PatrolState<EnemyStates> patrolState;
        private AlertState<EnemyStates> alertState;
        private AttackState<EnemyStates> attackState;
        private DamagedState<EnemyStates> damagedState;

        private void Awake()
        {
            InitFSM();

            damageable.OnTakeDamage += OnTakeDamageHandler;
            damagedState.onTimerEnded += OnTimerEndedHandler;

            fov.ToggleFindingTargets(true);
        }

        private void OnEnable()
        {
            hitsManager.OnParried += OnParriedHandler;
        }

        private void OnDisable()
        {
            hitsManager.OnParried -= OnParriedHandler;
        }

        private void InitFSM()
        {
            fsm = new FiniteStateMachine<EnemyStates>();

            Transform trans = transform;
            patrolState = new PatrolState<EnemyStates>(rb, EnemyStates.Patrol, "PatrolState", groundCheckPoint, trans, settings.patrolSettings);
            patrolState.SetDirection(1.0f);
            alertState = new AlertState<EnemyStates>(rb, EnemyStates.Alert, "AlertState", trans, settings.alertSettings);
            attackState = new AttackState<EnemyStates>(EnemyStates.Attack, "AttackState", hitsManager.gameObject);
            damagedState = new DamagedState<EnemyStates>(EnemyStates.Damaged, "DamagedState", EnemyStates.Alert, 2.0f, 4.0f, rb);

            fsm = new FiniteStateMachine<EnemyStates>();

            fsm.AddState(patrolState);
            fsm.AddState(alertState);
            fsm.AddState(attackState);
            fsm.AddState(damagedState);

            fsm.SetCurrentState(fsm.GetState(startingState));

            fsm.Init();
        }

        private void Update()
        {
            fsm.Update();

            CheckRotation();
            CheckFieldOfView();
            CheckTransitions();
        }

        private void FixedUpdate()
        {
            fsm.FixedUpdate();
        }

        private void CheckFieldOfView()
        {

            if (fov.visibleTargets.Count <= 0)
            {
                suspectMeter -= suspectUnit * Time.deltaTime;
            }
            else
            {
                suspectMeter += suspectUnit *
                            Mathf.Clamp(
                                fov.viewRadius - Vector3.Distance(fov.visibleTargets[0].transform.position,
                                    transform.position), 0, fov.viewRadius) * Time.deltaTime;

                if (suspectMeter >= settings.alertValue)
                {
                    if (detectedPlayer == null)
                        detectedPlayer = fov.visibleTargets[0];
                }
                else
                {
                    detectedPlayer = null;
                    suspectMeterSprite.color = Color.white;
                }
            }

            if (suspectMeter >= settings.alertValue)
            {
                suspectMeterSprite.color = Color.yellow;
            }
            else if(suspectMeter < settings.suspectMeterMaximum)
            {
                suspectMeterSprite.color = Color.white;
            }

            suspectMeter = Mathf.Clamp(suspectMeter, settings.suspectMeterMinimum, settings.suspectMeterMaximum);


            var normalizedSuspectMeter = (suspectMeter - (settings.suspectMeterMinimum)) /
                                         ((settings.suspectMeterMaximum) - (settings.suspectMeterMinimum));

            suspectMeterMask.transform.localPosition = new Vector3(0.0f,
                Mathf.Lerp(-0.798f, 0.078f, (0.078f - (-0.798f)) * normalizedSuspectMeter), 0.0f);
        }

        private void CheckTransitions()
        {
            if (fsm.GetCurrentState() == damagedState)
                return;

            if (fsm.GetCurrentState() == attackState && !attackState.Active)
                fsm.SetCurrentState(alertState);

            if (fov.visibleTargets.Count > 0)
            {
                Transform viewedTarget = fov.visibleTargets[0];
                if (suspectMeter >= settings.suspectMeterMaximum)
                {
                    suspectMeterSprite.color = Color.red;
                    turnedAggro = true;
                    if (!(Vector3.Distance(viewedTarget.position, transform.position) < hitDistance)) return;

                    attackState.Enter();
                    fsm.SetCurrentState(attackState);
                }
                else if (suspectMeter >= settings.alertValue)
                {
                    alertState.SetTarget(viewedTarget);
                    fsm.SetCurrentState(alertState);

                    suspectMeterSprite.color = Color.yellow;
                }
            }
            else if (detectedPlayer == null && !turnedAggro)
            {
                fsm.SetCurrentState(patrolState);
            }
        }

        private void CheckRotation()
        {
            if (fsm.GetCurrentState() == patrolState)
            {
                switch (patrolState.dir)
                {
                    case > 0:
                        {
                            if (!facingRight)
                            {
                                Flip();
                            }
                            break;
                        }
                    case < 0:
                        {
                            if (facingRight)
                            {
                                Flip();
                            }
                            break;
                        }
                }
            }
            else if (fsm.GetCurrentState() == alertState)
            {
                if (alertState.dir > 0)
                {
                    if (!facingRight)
                    {
                        Flip();
                    }
                }
                else if (alertState.dir < 0)
                {
                    if (facingRight)
                    {
                        Flip();
                    }
                }
            }
            else if (fsm.GetCurrentState() == attackState)
            {
                if (fov.visibleTargets.Count > 0)
                {
                    var targetPos = fov.visibleTargets[0].transform.position;

                    if (targetPos.x > transform.position.x && !facingRight)
                        Flip();
                    else if (targetPos.x < transform.position.x && facingRight)
                        Flip();
                }

            }
        }

        private void OnTakeDamageHandler(Vector2 origin)
        {
            if (origin.x > transform.position.x && !facingRight)
                Flip();
            else if (origin.x < transform.position.x && facingRight)
                Flip();

            Vector2 pushDirection = facingRight ? Vector2.left : Vector2.right;

            damagedState.SetDirection(pushDirection);

            if (fsm.GetCurrentState() != damagedState)
                fsm.SetCurrentState(damagedState);
            else
                damagedState.ResetState();
        }

        private void OnTimerEndedHandler(EnemyStates nextId)
        {
            fsm.SetCurrentState(fsm.GetState(nextId));
        }

        private void OnParriedHandler()
        {
            fsm.SetCurrentState(damagedState);
            damagedState.SetDirection(facingRight ? -transform.right : transform.right);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawRay(groundCheckPoint.position, groundCheckPoint.right * patrolState.dir);
        }
    }
}