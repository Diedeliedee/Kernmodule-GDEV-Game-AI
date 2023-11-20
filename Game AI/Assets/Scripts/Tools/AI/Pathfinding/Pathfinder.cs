using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Joeri.Tools.Utilities;

namespace Joeri.Tools.AI.Pathfinding
{
    /// <summary>
    /// Simple A* pathfinder
    /// </summary>
    public partial class Pathfinder
    {
        private readonly ValidNodeCheck m_nodeEvaluation    = null;
        private readonly Vector2Int[] m_validDirections     = null;

        public Pathfinder(ValidNodeCheck _nodeEvaluation, Vector2Int[] _validDirections)
        {
            m_nodeEvaluation    = _nodeEvaluation;
            m_validDirections   = _validDirections;
        }

        public Path FindPath(Vector2Int start, Vector2Int goal)
        {
            return GetPathResult(start, goal).path;
        }

        public Result GetPathResult(Vector2Int start, Vector2Int goal)
        {
            var openNodes   = new Dictionary<Vector2Int, Node>();
            var closedNodes = new Dictionary<Vector2Int, Node>();

            var currentNode = new Node(start, goal);

            /// Adds the passed in node to the open Nodes dictionary.
            void AddNodeToOpen(Node node)
            {
                openNodes.Add(node.coordinates, node);
            }

            /// Checks if the passed in coordinates have a Node in the open dictionary, and switches the Node to the closed dictionary.
            void SwitchNodeToClosed(Node node)
            {
                openNodes   .Remove(node.coordinates);
                closedNodes .Add(node.coordinates, node);
            }

            /// Returns the Node with the lowest f-cost in the open Nodes dictionary.
            Node FindLowestFCost()
            {
                Node lowest = openNodes.First().Value;

                foreach (var node in openNodes)
                {
                    var lowerFCost = node.Value.fCost < lowest.fCost;
                    var lowerHCost = node.Value.fCost == lowest.fCost && node.Value.hCost < lowest.hCost;

                    if (!lowerFCost && !lowerHCost) continue;
                    lowest = node.Value;
                }
                return lowest;
            }

            /// Returns a dictionary of all the neighboring coordinates of the current Node.
            List<Vector2Int> GetNeighboringCoordinates()
            {
                var coordinates = new List<Vector2Int>();

                for (int i = 0; i < m_validDirections.Length; i++)
                {
                    coordinates.Add(currentNode.coordinates + m_validDirections[i]);
                }
                return coordinates;
            }

            /// Returns a Path from the start to the current node.
            Path RetraceNodes()
            {
                var coordinatesBundle   = new List<Vector2Int>();
                var crumb               = currentNode;

                while (true)
                {
                    coordinatesBundle.Add(crumb.coordinates);
                    if (crumb.coordinates == start)
                    {
                        coordinatesBundle.Add(crumb.coordinates);
                        break;
                    }
                    crumb = crumb.parent;
                }
                coordinatesBundle.Reverse();

                return new Path(coordinatesBundle.ToArray());
            }

            AddNodeToOpen(currentNode);
            while (true)
            {
                //  Exit the loop if there is no open path to the goal.
                if (openNodes.Count <= 0)
                {
                    Debug.Log($"Pathfinding failed with zero remaining open nodes. {closedNodes.Count} nodes have been closed in the process.");
                    return new Result(null, openNodes, closedNodes);
                }

                //  Find the Node with the lowest f-cost, and switch it to the closed Nodes dictionary.
                currentNode = FindLowestFCost();
                SwitchNodeToClosed(currentNode);

                //  Return out of the loop if we reached the goal.
                if (currentNode.coordinates == goal)
                {
                    return new Result(RetraceNodes(), openNodes, closedNodes);
                }

                //  Check every valid neighbor in the surroundings of the current node, and add the qualified nodes to the open dictionary.
                foreach (var coordinates in GetNeighboringCoordinates())
                {
                    //  If the neighbor's coordinates cannot bear a valid node, or already bears a node in the closed dictionary, skip the iteration.
                    if (!IsValidNode(coordinates) || closedNodes.ContainsKey(coordinates)) continue;

                    var newNeighbor = new Node(coordinates, goal, currentNode);

                    //  If a node already exists in with the neighboring coordinates..
                    if (openNodes.TryGetValue(coordinates, out Node neighboringNode))
                    {
                        //  Skip if the path through the newly created node is longer than the existing neighboring node.
                        if (newNeighbor.gCost >= neighboringNode.gCost) continue;

                        //  Otherwise, remove the existing node first, and..
                        openNodes.Remove(coordinates);
                    }

                    //  Add the newly created node in the open dictionary.
                    AddNodeToOpen(newNeighbor);
                }
            }
        }

        /// <returns>True if the passed in coordinates could harbor a valid Node. False if not.</returns>
        private bool IsValidNode(Vector2Int coordinates)
        {
            return m_nodeEvaluation(coordinates);
        }
    
        public delegate bool ValidNodeCheck(Vector2Int coordinates);
    }
}