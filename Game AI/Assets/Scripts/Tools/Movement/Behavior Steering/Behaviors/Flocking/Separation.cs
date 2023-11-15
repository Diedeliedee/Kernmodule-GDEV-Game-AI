using UnityEngine;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Movement
{
    public class Separation : Behavior
    {
        private readonly IBoid m_boid = null;
        private readonly IFlock m_flock = null;
        private readonly float m_radius = 0;
        private readonly float m_sqrRadius = 0;
        private readonly float m_force = 0;

        public Separation(float _radius, float _force, IBoid _boid, IFlock _flock)
        {
            m_boid = _boid;
            m_flock = _flock;
            m_radius = _radius;
            m_sqrRadius = _radius * _radius;
            m_force = _force;
        }

        public override Vector3 GetDesiredVelocity(Context context)
        {
            //  Collect all boids in the same flock as this one.
            var peerBoids = m_flock.GetPeerBoids(m_boid);

            //  If no peer boids have been found, don't add velocity.
            if (peerBoids.Count <= 0) return Vector3.zero;

            //  Variables for caching.
            var totalPositions = Vector3.zero;
            var boidsInRange = 0;

            for (int i = 0; i < peerBoids.Count; i++)
            {
                //  Do not include boids further than the effective radius of this behavior.
                if ((peerBoids[i].position - context.position).sqrMagnitude > m_sqrRadius) continue;

                //  Add the position of the boid to be used in the average equation.
                totalPositions += peerBoids[i].position;
                boidsInRange++;
            }

            //  Calculate desired velocity away from the average of all nearby boids.
            totalPositions /= boidsInRange;
            return (context.position - totalPositions).normalized * m_force;
        }

        public override void DrawGizmos(Vector3 position)
        {
            GizmoTools.DrawSphere(position, m_radius, Color.red, 0.5f, true, 0.25f);
        }
    }
}