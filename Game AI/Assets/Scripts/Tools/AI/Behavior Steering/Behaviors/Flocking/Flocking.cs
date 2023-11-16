using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Joeri.Tools.AI.Steering
{
    public class Flocking : Behavior
    {
        private readonly IFlock m_flock = null;
        private readonly IBoid m_boid = null;

        private List<FlockRule> m_rules = new List<FlockRule>();

        public Flocking(IFlock _flock, IBoid _boid, params FlockRule[] _rules)
        {
            m_boid = _boid;
            m_flock = _flock;
            m_rules = _rules.ToList();
        }

        public void SetRules(params FlockRule[] _rules)
        {
            m_rules.Clear();
            m_rules = _rules.ToList();
        }

        public void ClearRules()
        {
            m_rules.Clear();
        }

        public override Vector3 GetDesiredVelocity(Context context)
        {
            var velocity = Vector3.zero;
            var peerBoids = m_flock.GetPeerBoids(m_boid);

            if (peerBoids.Count <= 0) return velocity;

            for (int i = 0; i < m_rules.Count; i++)
            {
                velocity += m_rules[i].GetContribution(context.deltaTime, context.position, peerBoids);
            }
            return velocity;
        }

        public override void DrawGizmos(Vector3 position)
        {
            for (int i = 0; i < m_rules.Count; i++) m_rules[i].DrawGizmos(position, m_flock.GetPeerBoids(m_boid));
        }
    }
}
