using System.Collections.Generic;
using UnityEngine;

public class CheckpointMemory
{
    public LinkedList<Transform> checkpoints = new();
    public LinkedListNode<Transform> currentCheckpoint = null;

    public CheckpointMemory(params Transform[] _checkpoints)
    {
        //  Adding checkpoints.
        for (int i = 0; i < _checkpoints.Length; i++) checkpoints.AddLast(_checkpoints[i]);

        //  Connecting ends.
        checkpoints.AddLast(checkpoints.First);

        //  Caching the current checkpoint.
        currentCheckpoint = checkpoints.First;
    }
}
