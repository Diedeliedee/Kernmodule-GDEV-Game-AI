using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Structure.StateMachine
{
    /// <summary>
    /// Class handling a class-based finite state machine system,
    /// </summary>
    public class FSM : IStateMachine
    {
        protected IState m_activeState = null;

        protected readonly Dictionary<System.Type, IState> m_states  = new Dictionary<System.Type, IState>();

        public FSM(params IState[] states)
        {
            foreach (var state in states)
            {
                state.Setup(this);
                m_states.Add(state.GetType(), state);
            }
            OnSwitch(states[0].GetType());
        }

        public virtual void Tick()
        {
            if (m_activeState == null)
            {
                Debug.LogError("Active state is not yet set. Possibly the Start() function has not been called yet.");
                return;
            }

            m_activeState.OnTick();
        }

        public virtual void OnSwitch(System.Type state)
        {
            m_activeState?.OnExit();
            try     { m_activeState = m_states[state]; }
            catch   { Debug.LogError($"The state: '{state.Name}' is not found within the state dictionary."); return; }
            m_activeState?.OnEnter();
        }

        /// <summary>
        /// Function to call the gizmos of the current active state.
        /// </summary>
        public virtual void DrawGizmos(Vector3 position)
        {
            void DrawLabel(string label)
            {
                GizmoTools.DrawLabel(position, label, Color.black);
            }

            if (m_activeState == null) return;
            //  Drawing text in the world describing the current state the agent is in.
            DrawLabel(m_activeState.GetType().Name);

            //  Drawing the gizmos of the current state, if it isn't null.
            ((State)m_activeState).OnDrawGizmos();
        }
    }
}