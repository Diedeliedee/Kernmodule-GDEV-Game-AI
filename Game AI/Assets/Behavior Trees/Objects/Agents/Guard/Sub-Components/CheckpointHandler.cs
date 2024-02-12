using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler
{
    public List<Transform> checkpoints = new();
    public int index = 0;

    public CheckpointHandler(params Transform[] _checkpoints)
    {
        for (int i = 0; i < _checkpoints.Length; i++) checkpoints.Add(_checkpoints[i]);
    }

    public Transform GetNext()
    {
        ++index;
        if (index >= checkpoints.Count) index = 0;
        return checkpoints[index];
    }
}
