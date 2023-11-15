using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Utilities;

public class Flock : MonoBehaviour, IFlock
{
    [Header("Properties")]
    [SerializeField] private GameObject m_boid;
    [SerializeField] private int m_boidAmount = 5;
    [SerializeField] private float m_spawnRadius = 5f;

    private Dictionary<int, Boid> m_boids = new Dictionary<int, Boid>();

    private void Start()
    {
        Boid[] boidArray;

        //  Spawn the boids.
        for (int i = 0; i < m_boidAmount; i++)
        {
            Instantiate(m_boid, transform.position + Vectors.RandomSpherePoint(m_spawnRadius), Quaternion.identity, transform);
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
    }
}
