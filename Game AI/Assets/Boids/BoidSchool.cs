using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSchool : MonoBehaviour
{
    private Dictionary<int, Boid> m_boids = new Dictionary<int, Boid>();

    private void Start()
    {
        var boidArray = GetComponentsInChildren<Boid>();

        //  Add all child boids in a dictionary with the instance ID as a key.
        foreach (var boid in boidArray)
        {
            m_boids.Add(boid.GetInstanceID(), boid);
            boid.Setup(this);
        }
    }

    private void Update()
    {
        foreach (var pair in m_boids) pair.Value.Tick(Time.deltaTime);
    }

    public List<Boid> GetBoids(Boid exclude)
    {
        var boidList = new List<Boid>();

        foreach (var pair in m_boids)
        {
            if (pair.Key == exclude.GetInstanceID()) continue;
            boidList.Add(pair.Value);
        }
        return boidList;
    }

    private void OnDrawGizmos()
    {
        foreach (var pair in m_boids) pair.Value.DrawGizmos();
    }
}
