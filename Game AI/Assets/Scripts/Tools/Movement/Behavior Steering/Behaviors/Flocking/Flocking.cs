using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.Movement
{
    public partial class Flocking : Behavior
    {
        private Dictionary<int, IBoid> m_boids = new Dictionary<int, IBoid>();

        public Flocking(IBoid[] _boids, IBoid exclusion)
        {
            foreach (var boid in _boids)
            {
                if (boid.GetHashCode() == exclusion.GetHashCode()) continue;
                m_boids.Add(boid.GetHashCode(), boid);
            }
        }

        public override Vector3 GetDesiredVelocity(Context context)
        {

        }
    }

    public interface IBoid
    {
        public Vector3 Velocity { get; }

        public Collider Collider { get; }
    }
}