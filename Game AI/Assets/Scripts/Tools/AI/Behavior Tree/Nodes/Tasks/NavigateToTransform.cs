using Joeri.Tools.Movement.ThreeDee;
using UnityEngine;
using UnityEngine.AI;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class NavigateToTransform : LeafNode
    {
        public readonly NavMeshAgent m_agent = null;
        public readonly Transform m_target = null;

        public NavigateToTransform(NavMeshAgent _agent, Transform _target)
        {
            m_agent = _agent;
            m_target = _target;
        }

        public override void OnEnter()
        {
            if (m_agent.SetDestination(m_target.position));
        }

        public override State OnUpdate()
        {
            if (m_agent.pathPending || m_agent.remainingDistance > 0) return State.Running;
            return State.Succes;
        }

        public override void OnExit()
        {
            m_agent.ResetPath();
        }
    }
}
