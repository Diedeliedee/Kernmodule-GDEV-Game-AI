using Joeri.Tools.Movement.ThreeDee;
using UnityEngine;
using UnityEngine.AI;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class NavigateToTarget : LeafNode
    {
        public NavMeshAgent m_agent = null;

        public override void OnEnter()
        {
            m_agent = board.Get<NavMeshAgent>();
            m_agent.SetDestination(board.Get<TargetMemory>().target);
        }

        public override State OnUpdate()
        {
            if (m_agent.pathPending || m_agent.remainingDistance > 0) return State.Running;
            return State.Succes;
        }

        public override void OnExit()
        {
            m_agent.ResetPath();
            m_agent = null;
        }
    }
}
