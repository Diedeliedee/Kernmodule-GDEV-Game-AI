using Joeri.Tools.AI.Steering;
using Joeri.Tools.Movement;
using Joeri.Tools.Utilities;
using UnityEngine;

namespace GameAI.Boids
{
    public class Boid : MonoBehaviour, IBoid
    {
        [Header("Properties:")]
        [SerializeField] private BoidSettings m_settings;

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
                new Flocking(m_flock, this,
                    new Cohesion(m_settings.cohesionDistance, m_settings.cohesionForce),
                    new Separation(m_settings.seperationDistance, m_settings.seperationForce),
                    new Alignment(m_settings.cohesionDistance)),
                new MoveForward(m_settings.travelSpeed, transform),
                new MoveToPoint(m_flock.transform.position, m_settings.centerAttractionForce),
                new StayInArea(m_flock.transform.position, m_flock.areaRadius, m_settings.boundaryOpposingForce));

            transform.LookAt(transform.position += Util.RandomSpherePoint(1f).normalized);
        }

        public void Tick(float _deltaTime)
        {
            var resultVelocity = m_movement.CalculateVelocity(
            m_behaviorHandler.GetDesiredVelocity(
                new Context(
                    _deltaTime,
                    m_settings.travelSpeed,
                    position,
                    velocity)),
            m_settings.grip,
            _deltaTime);

            transform.position += resultVelocity * _deltaTime;
            transform.LookAt(transform.position + resultVelocity);
        }

        public void DrawGizmos()
        {
            m_behaviorHandler.DrawGizmos(transform.position);
        }
    }
}