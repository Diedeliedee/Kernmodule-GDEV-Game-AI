using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Joeri.Tools.Structure.StateMachine.Advanced
{
    public class CompositeFSM<T> : IStateMachine
    {
        private CompositeState<T> m_activeRootState                 = null;
        private Dictionary<Type, CompositeState<T>> m_rootStates    = new Dictionary<Type, CompositeState<T>>();

        public CompositeState<T> activeState { get => m_activeRootState; }

        public CompositeFSM(T source, params CompositeState<T>[] states)
        {
            foreach (var state in states)
            {
                m_rootStates.Add(state.GetType(), state);

                state.Setup(this);
                state.RelaySource(source);
            }
            OnSwitch(states[0].GetType());
        }

        public void Tick()
        {
            m_activeRootState.Tick();
        }

        public void OnSwitch(Type state)
        {
            m_activeRootState?.Reset();
            m_activeRootState?.OnExit();

            try     { m_activeRootState = m_rootStates[state]; }
            catch   { Debug.LogError($"The state: '{state.Name}' is not found within the state dictionary."); return; }

            m_activeRootState.OnEnter();
            m_activeRootState.Activate();
        }
    } 
}
