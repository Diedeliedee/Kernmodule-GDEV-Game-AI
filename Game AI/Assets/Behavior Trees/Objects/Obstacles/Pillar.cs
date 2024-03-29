﻿using Joeri.Tools.Utilities;
using UnityEngine;

namespace GameAI.BehaviorSystem
{
    public class Pillar : MonoBehaviour, IHidingCover
    {
        private CapsuleCollider m_collider = null;

        public Vector3 position => transform.position;

        private void Awake()
        {
            m_collider = GetComponentInChildren<CapsuleCollider>();
        }

        public Vector3 GetHidingPosition(Vector3 _from, out Vector3 _normal)
        {
            _normal = (transform.position.Planar() - _from.Planar()).normalized.Cubular();
            var pos = transform.position + _normal * m_collider.radius;
            pos.y = 0;
            return pos;
        }
    }
}