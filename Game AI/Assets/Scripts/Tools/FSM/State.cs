﻿using System;

namespace Joeri.Tools.Structure.StateMachine
{
    public abstract class State : IState
    {
        /// <summary>
        /// The state machine this state is a part of.
        /// </summary>
        protected IStateMachine owner { get; private set; }

        public virtual void OnEnter()   { }

        public virtual void OnTick()    { }

        public virtual void OnExit()    { }

        public virtual void Switch(Type state)
        {
            owner.OnSwitch(state);
        }

        /// <summary>
        /// Called whenever the finite state machine the state is in, is created.
        /// </summary>
        public virtual void Setup(IStateMachine owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Abstract function allowing for gizmos to be drawn by the state machine.
        /// </summary>
        public virtual void OnDrawGizmos() { }
    }
}
