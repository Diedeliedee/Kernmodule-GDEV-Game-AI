using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Utilities;
using System;

namespace Joeri.Tools.Structure.StateMachine.Advanced
{
    public abstract class CompositeState<T> : IState, IStateMachine
    {
        //  Relations:
        private IStateMachine m_parent                          = null;
        private Dictionary<Type, CompositeState<T>> m_children  = null;

        //  Run-time:
        private CompositeState<T> m_activeChild     = null;
        private CompositeState<T> m_defaultChild    = null;

        //  Properties:
        protected T source                  { get; private set; }

        /// <summary>
        /// Create a composite state with not branching states.
        /// </summary>
        public CompositeState() { }

        /// <summary>
        /// Create a composite state with branching states.
        /// </summary>
        public CompositeState(params CompositeState<T>[] children)
        {
            m_children = new Dictionary<Type, CompositeState<T>>();
            foreach (var child in children)
            {
                m_children.Add(child.GetType(), child);
                child.m_parent = this;
            }
            m_defaultChild  = children[0];
        }

        #region FSM Functions
        public void Tick()
        {
            OnTick();
            m_activeChild?.Tick();
        }

        public void OnSwitch(Type state)
        {
            m_activeChild?.Reset();
            m_activeChild?.OnExit();

            try     { m_activeChild = m_children[state]; }
            catch   { Debug.LogError($"The child state: '{state.Name}' is not found within the state's lower hierarchy."); return; }

            m_activeChild.OnEnter();
            m_activeChild.Activate();
            Debug.Log($"Entered state: '{state.Name}'.");
        }
        #endregion

        #region State Functions
        public void Setup(IStateMachine parent)
        {
            m_parent = parent;
        }

        public virtual void OnEnter()   { }

        public virtual void OnTick()    { }

        public virtual void OnExit()    { }

        public void Switch(Type state)
        {
            m_parent.OnSwitch(state);
        }
        #endregion

        #region Composite Functions
        /// <summary>
        /// Activates this state's FSM to it's default state.
        /// </summary>
        public void Activate()
        {
            if (m_children == null)     return;
            if (m_activeChild != null)  return;

            m_activeChild = m_defaultChild;
            m_activeChild.OnEnter();
            m_activeChild.Activate();
        }

        /// <summary>
        /// Resets this state's FSM to it's default state.
        /// </summary>
        public void Reset()
        {
            if (m_children == null)     return;
            if (m_activeChild == null)  return;

            m_activeChild.Reset();
            m_activeChild.OnExit();

            m_activeChild = null;
        }

        /// <summary>
        /// Recursively register the source instance to this state in this, and all child states.
        /// </summary>
        public void RelaySource(T source)
        {
            this.source = source;

            if (m_children == null) return;
            foreach (var child in m_children)
            {
                child.Value.RelaySource(source);
            }
        }
        #endregion
    }
}