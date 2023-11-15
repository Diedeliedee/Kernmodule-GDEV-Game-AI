using UnityEngine;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Movement
{
    public class Alignment : Behavior
    {
        private readonly IBoid m_boid = null;
        private readonly IFlock m_flock = null;
        private readonly float m_sqrRadius = 0;

        public Alignment(float _radius, IBoid _boid, IFlock _flock)
        {
            m_boid = _boid;
            m_flock = _flock;
            m_sqrRadius = _radius * _radius;
        }

        public override Vector3 GetDesiredVelocity(Context context)
        {
            //  Collect all boids in the same flock as this one.
            var peerBoids = m_flock.GetPeerBoids(m_boid);

            //  If no peer boids have been found, don't add velocity.
            if (peerBoids.Count <= 0) return Vector3.zero;

            //  Variables for caching.
            var totalVelocity = Vector3.zero;
            var boidsInRange = 0;

            for (int i = 0; i < peerBoids.Count; i++)
            {
                //  Do not include boids further than the effective radius of this behavior.
                if ((peerBoids[i].position - context.position).sqrMagnitude > m_sqrRadius) continue;

                //  Add the position of the boid to be used in the average equation.
                totalVelocity += peerBoids[i].velocity;
                boidsInRange++;
            }

            //  Calculate and return the average velocity of all nearby boids.
           return totalVelocity /= boidsInRange;
        }
    }
}