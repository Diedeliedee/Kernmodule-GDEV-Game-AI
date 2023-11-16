using System;

namespace Joeri.Tools.Structure.StateMachine
{
    public interface IState
    {
        /// <summary>
        /// Called by the state machine's constructor.
        /// </summary>
        public void Setup(IStateMachine owner);

        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        public void OnEnter();

        /// <summary>
        /// Called by the state machine whenever Tick() is called.
        /// </summary>
        public void OnTick();

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        public void OnExit();

        /// <summary>
        /// Requests the state machine to switch to another state based on the passed in type parameter.
        /// </summary>
        public void Switch(Type stateType);
    }
}
