using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.AI.Pathfinding;

namespace GameAI.Pathfinding
{
    public class Agent : MonoBehaviour
    {
        public int moveButton = 0;
        public float moveSpeed = 3;

        //  Components:
        private MazeGeneration maze;
        private List<Vector2Int> path = new List<Vector2Int>();

        private Plane ground = new Plane(Vector3.up, 0f);

        //  Reference:
        private MeshRenderer renderer;
        private GameObject targetVisual;
        private LineRenderer line;


        private void Awake()
        {
            maze = FindObjectOfType<MazeGeneration>();
            renderer = GetComponentInChildren<MeshRenderer>();

            targetVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            targetVisual.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            targetVisual.GetComponent<MeshRenderer>().material.color = renderer.material.color;

            line = GetComponent<LineRenderer>();
            line.material.color = renderer.material.color;
            line.material.color = renderer.material.color;
        }

        private void Start()
        {
        }

        public void FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
        {
            var validDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            var pathFinder = new AStar(TileEvaluation, validDirections);

            path = pathFinder.GetPath(startPos, endPos);
            DrawPath();

            //  Returns true if the 'from' coordinate to the 'to' coordinate is accesible.
            bool TileEvaluation(Vector2Int _from, Vector2Int _to)
            {
                if (_to.x < 0 || _to.x >= grid.GetLength(0)) return false;
                if (_to.y < 0 || _to.y >= grid.GetLength(1)) return false;
                return !grid[_to.x, _to.y].HasWall(DirectionToWall(_to - _from));
            }

            //  Returns the first wall enum that the given direction wouldn't be able to pass through. 
            Wall DirectionToWall(Vector2Int _dir)
            {
                if (_dir.y < 0) return Wall.UP;
                if (_dir.y > 0) return Wall.DOWN;
                if (_dir.x > 0) return Wall.LEFT;
                return Wall.RIGHT;
            }
        }

        private void DrawPath()
        {
            if (path != null && path.Count > 0)
            {
                line.positionCount = path.Count;
                for (int i = 0; i < path.Count; i++)
                {
                    line.SetPosition(i, Vector2IntToVector3(path[i], 0.1f));
                }
            }
        }

        public void Update()
        {
            //  Move to clicked position.
            if (Input.GetMouseButtonDown(moveButton))
            {
                Debug.Log("Click");
                Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));

                Vector3 mousePos = MouseToWorld();
                Vector2Int targetPos = Vector3ToVector2Int(mousePos);
                targetVisual.transform.position = Vector2IntToVector3(targetPos);
                FindPathToTarget(Vector3ToVector2Int(transform.position), targetPos, maze.grid);
            }

            if (path != null && path.Count > 0)
            {
                if (transform.position != Vector2IntToVector3(path[0]))
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector2IntToVector3(path[0]) - transform.position), 360f * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, Vector2IntToVector3(path[0]), moveSpeed * Time.deltaTime);
                }
                else
                {
                    path.RemoveAt(0);
                    DrawPath();
                }
            }
        }
        public Vector3 MouseToWorld()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distToGround = -1f;
            ground.Raycast(ray, out distToGround);
            Vector3 worldPos = ray.GetPoint(distToGround);

            return worldPos;
        }

        private Vector2Int Vector3ToVector2Int(Vector3 pos)
        {
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        }

        private Vector3 Vector2IntToVector3(Vector2Int pos, float YPos = 0)
        {
            return new Vector3(Mathf.RoundToInt(pos.x), YPos, Mathf.RoundToInt(pos.y));
        }

        private void OnDrawGizmos()
        {
            if (path == null || path.Count <= 0) return;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.color = renderer.material.color;
                Gizmos.DrawLine(Vector2IntToVector3(path[i], 0.5f), Vector2IntToVector3(path[i + 1], 0.5f));
            }
        }
    }
}