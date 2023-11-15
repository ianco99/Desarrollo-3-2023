using Code.SOs.States;

namespace Patterns.FSM
{
    public class CooldownState<T> : TimerTransitionState<T>
    {
        public float Timer => currentTimer;

        public CooldownState(T id, T nextStateID, TimerSettings settings) : base(id, nextStateID, settings)
        {
        }
    }
}
