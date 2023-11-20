using UnityEngine;

namespace Joeri.Tools.AI.Pathfinding
{
    public class Node
    {
        public readonly Node parent = null;
        public readonly Vector2Int coordinates;
    
        public readonly int gCost = 0;
        public readonly int hCost = 0;
        public readonly int fCost = 0;
    
        public Node(Vector2Int coordinates, Vector2Int goal, Node parent = null)
        {
            this.parent         = parent;
            this.coordinates    = coordinates;
    
            if (parent != null) gCost = parent.gCost + Mathf.RoundToInt((coordinates - parent.coordinates).magnitude * 10);
            else                gCost = 0;
    
            hCost = Mathf.RoundToInt((goal - coordinates).magnitude * 10);
            fCost = gCost + hCost;
        }
    }
}