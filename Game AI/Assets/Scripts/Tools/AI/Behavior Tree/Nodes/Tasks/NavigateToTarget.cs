﻿using Joeri.Tools.Movement.ThreeDee;
using UnityEngine;
using UnityEngine.AI;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class NavigateToTarget : LeafNode
    {
        private NavMeshAgent m_agent = null;
        private TargetMemory m_memory = null;

        public override void OnEnter()
        {
            m_memory ??= board.Get<TargetMemory>();

            m_agent = board.Get<NavMeshAgent>();
            m_agent.SetDestination(m_memory.target);
        }

        public override State OnUpdate()
        {
            if (m_agent.pathPending || m_agent.remainingDistance > m_memory.epsilon) return State.Running;
            return State.Succes;
        }

        public override void OnExit()
        {
            m_agent.ResetPath();
            m_agent = null;
        }
    }
}
