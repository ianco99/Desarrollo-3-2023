using System;
using System.Collections;
using System.Collections.Generic;
using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class TimerTransitionState<T> : BaseState<T>
    {
        public Action<T> onTimerEnded;

        protected T nextStateID;
        protected TimerSettings timerSettings;

        protected float currentTimer;

        public TimerTransitionState(T id, T nextStateID, TimerSettings settings) : base(id)
        {
            this.nextStateID = nextStateID;
            timerSettings = settings;
        }
        
        public TimerTransitionState(T id, string name, T nextStateID, TimerSettings settings) : base(id, name)
        {
            this.nextStateID = nextStateID;
            timerSettings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            currentTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            currentTimer += Time.deltaTime;

            if (!(currentTimer > timerSettings.maxTime)) return;
            
            Exit();
            onTimerEnded?.Invoke(nextStateID);
        }
    }
}