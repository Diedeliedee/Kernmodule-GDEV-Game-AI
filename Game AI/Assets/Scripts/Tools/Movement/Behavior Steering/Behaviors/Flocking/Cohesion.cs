using UnityEngine;

namespace Joeri.Tools.Movement
{
    public class FlockCohesion : Behavior
    {
        private readonly float m_radius = 0;
        private readonly float m_force = 0;
        private readonly Collider m_exclusion = null;

        public FlockCohesion(float _radius, float _force, Collider _exclusion)
        {
            m_radius = _radius;
            m_force = _force;
            m_exclusion = _exclusion;
        }

        public override Vector3 GetDesiredVelocity(Context context)
        {
            var surroundingBoids = Physics.OverlapSphere(context.position, m_radius);

            foreach (var collider in surroundingBoids)
            {

            }

            if (m_neightborCount > 0)
            {
                //  Returning a direction towards the average position of all nearby boids.
                return (averageTotal - position).normalized;
            }

            return Vector3.zero;
        }
    }
}