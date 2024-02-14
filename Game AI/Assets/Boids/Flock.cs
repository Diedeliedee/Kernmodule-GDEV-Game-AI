using Joeri.Tools.AI.Steering;
using Joeri.Tools.Debugging;
using Joeri.Tools.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI.Boids
{
    public class Flock : MonoBehaviour, IFlock
    {
        [Header("Properties")]
        [SerializeField] private GameObject m_boid;
        [SerializeField] private int m_boidAmount = 5;
        [SerializeField] private float m_spawnRadius = 5f;

        private Dictionary<int, Boid> m_boids = new Dictionary<int, Boid>();

        public float areaRadius => m_spawnRadius;

        private void Start()
        {
            Boid[] boidArray;

            //  Spawn the boids.
            for (int i = 0; i < m_boidAmount; i++)
            {
                Instantiate(m_boid, transform.position + Util.RandomSpherePoint(m_spawnRadius), Quaternion.identity, transform);
            }

            //  Gather all boids in an array.
            boidArray = GetComponentsInChildren<Boid>();

            //  Add all child boids in a dictionary with the instance ID as a key.
            foreach (var boid in boidArray)
            {
                m_boids.Add(boid.GetInstanceID(), boid);
                boid.Setup(this);
            }
        }

        private void Update()
        {
            //  Tick the boids.
            foreach (var pair in m_boids) pair.Value.Tick(Time.deltaTime);
        }

        /// <returns>All boids of this flock, excluding the passed in instance of an IBoid.</returns>
        public List<IBoid> GetPeerBoids(IBoid _exclusion)
        {
            var boidList = new List<IBoid>();

            foreach (var pair in m_boids)
            {
                if (pair.Key == _exclusion.GetHashCode()) continue;
                boidList.Add(pair.Value);
            }
            return boidList;
        }

        private void OnDrawGizmos()
        {
            foreach (var pair in m_boids) pair.Value.DrawGizmos();
            GizmoTools.DrawSphere(transform.position, m_spawnRadius, Color.white, 0.1f, true, 0.25f);
        }
    }
}