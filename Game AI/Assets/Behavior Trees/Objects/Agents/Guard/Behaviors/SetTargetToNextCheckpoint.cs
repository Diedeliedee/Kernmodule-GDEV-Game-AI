using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joeri.Tools.AI.BehaviorTree
{
    public class SetTargetToNextCheckpoint : LeafNode
    {
        public override State OnUpdate()
        {
            var checkpointMemory = board.Get<CheckpointMemory>();

            board.Get<TargetMemory>().SetTarget(checkpointMemory.currentCheckpoint.Value.position);
            checkpointMemory.currentCheckpoint = checkpointMemory.currentCheckpoint.Next;
            return State.Succes;
        }
    }
}
