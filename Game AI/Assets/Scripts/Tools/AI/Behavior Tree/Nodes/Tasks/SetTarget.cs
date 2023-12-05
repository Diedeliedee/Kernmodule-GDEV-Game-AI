using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class SetTarget : LeafNode
    {
        private Vector3 m_target = default;

        public SetTarget(Vector3 _target)
        {
            m_target = _target;
        }

        public override State OnUpdate()
        {
            board.Get<TargetMemory>().SetTarget(m_target);
            return State.Succes;
        }

        public override void OnAbort()
        {
            board.Get<TargetMemory>().Reset();
        }
    }
}
