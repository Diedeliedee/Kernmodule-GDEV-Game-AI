using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Debugging;

public class Boid : MonoBehaviour, IBoid
{
    [Header("Properties:")]
    [SerializeField] private float m_maxSpeed = 10;
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
    private Flock m_flock = null;

    public Vector3 position => transform.position;
    public Vector3 velocity => m_movement.velocity;

    public void Setup(Flock _flock)
    {
        m_flock = _flock;
        m_behaviorHandler.SetBehaviors(
            new Cohesion(m_cohesionDistance, m_cohesionForce, this, m_flock),
            new Separation(m_seperationDistance, m_seperationForce, this, m_flock),
            new Alignment(m_cohesionDistance, this, m_flock));
    }

    public void Tick(float _deltaTime)
    {
        var context = new Behavior.Context(_deltaTime, m_maxSpeed, position, velocity);

        transform.position += m_movement.CalculateVelocity(m_behaviorHandler.GetDesiredVelocity(context), m_grip, _deltaTime) * _deltaTime;
    }

    public void DrawGizmos()
    {
        m_behaviorHandler.DrawGizmos(transform.position);
        m_movement.Draw(transform.position, Color.black, Color.green, 0.25f);
    }
}
