using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Joeri.Tools.AI.Pathfinding
{
    public interface IAlgorithm
    {
        public LinkedList<Vector2Int> GetPath(Vector2Int _start, Vector2Int _end);
    }
}
