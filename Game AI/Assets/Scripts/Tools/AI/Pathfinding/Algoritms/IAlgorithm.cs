using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.AI.Pathfinding
{
    public interface IAlgorithm
    {
        public LinkedList<Vector2Int> GetPath(Vector2Int _start, Vector2Int _end);
    }
}
