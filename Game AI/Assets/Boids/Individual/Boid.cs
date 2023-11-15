using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Debugging;

public class Boid : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private float m_grip = 3;
    [Space]
    [SerializeField] private float m_cohesionDistance = 3;
    [SerializeField] private float m_cohesionForce = 3;
    [Space]
    [SerializeField] private float m_seperationDistance = 3;
    [SerializeField] private float m_seperationForce = 3;

    //  Components:
    private Accel.Omni m_movement = new Accel.Omni();
    private BehaviorHandler m_behaviorHandler = new BehaviorHandler();

    //  Reference:
    private BoidSchool m_parentSchool = null;

    public void Setup(BoidSchool _school)
    {

        m_parentSchool = _school;
    }

    public void Tick(float _deltaTime)
    {
        var boids = m_parentSchool.GetBoids(this);
        var desiredVelocity = Vector3.zero;

        Isolate(boids, m_cohesionDistance);
        foreach (var boid in boids)
        {
            desiredVelocity += Vector3.ClampMagnitude((boid.transform.position - transform.position).normalized * m_cohesionForce, m_cohesionForce);
        }

        Isolate(boids, m_seperationDistance);
        foreach (var boid in boids)
        {
            desiredVelocity += Vector3.ClampMagnitude((transform.position - boid.transform.position).normalized * m_seperationForce, m_seperationForce);
        }

        transform.position += m_movement.CalculateVelocity(desiredVelocity, m_grip, _deltaTime) * _deltaTime;
    }

    private void Isolate(List<Boid> boids, float radius)
    {
        //  Square the radius, so we avoid using a square root function in the distance calculation.
        radius = radius * radius;

        //  Remove any boids out of the desired radius of this boid.
        for (int i = 0; i < boids.Count; i++)
        {
            if ((boids[i].transform.position - transform.position).sqrMagnitude > radius)
            {
                boids.Remove(boids[i]);
                i--;
            }
        }
    }

    public void DrawGizmos()
    {
        GizmoTools.DrawSphere(transform.position, m_cohesionDistance, Color.white, 0.5f, true, 0.25f);
        GizmoTools.DrawSphere(transform.position, m_seperationDistance, Color.red, 0.5f, true, 0.25f);
    }
}
