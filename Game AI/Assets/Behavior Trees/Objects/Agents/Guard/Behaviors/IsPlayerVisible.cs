using UnityEngine;
using Joeri.Tools.AI.BehaviorTree;

public class IsPlayerVisible : LeafNode
{
    private Transform m_pointOfOrigin = null;
    private float m_range = 0f;
    private float m_angle = 0f;
    private int m_resolution = 0;

    public IsPlayerVisible(Transform _pointOfOrigin, float _range, float _angle, int _resolution)
    {
        m_pointOfOrigin = _pointOfOrigin;
        m_range = _range;
        m_angle = _angle;
        m_resolution = _resolution;
    }

    public override State OnUpdate()
    {
        var cachedTransformData = m_pointOfOrigin.localRotation;
        var rotationPerIteration = m_angle / m_resolution;

        m_pointOfOrigin.Rotate(0f, -m_angle / 2f, 0f, Space.Self);
        for (int i = 0; i < m_resolution; i++)
        {
            //  Try to detect player.
            if (!Physics.Raycast(m_pointOfOrigin.position, m_pointOfOrigin.forward, out RaycastHit _hit, m_range))  { Iterate(); continue; }
            if (!_hit.transform.TryGetComponent(out Player _player))                                                { Iterate(); continue; }

            //  Draw debug ray, and cleanup transform.
            Debug.DrawRay(m_pointOfOrigin.position, m_pointOfOrigin.forward * _hit.distance);
            m_pointOfOrigin.localRotation = cachedTransformData;

            //  Register threat memory.
            var threatMemory = board.Get<ThreatMemory>();

            threatMemory.hasSeenThreat = true;
            threatMemory.lastSeenThreatLocation = _hit.transform.position;

            //  Return succes.
            return State.Succes;
        }

        //  Return failure if no player has been found.
        m_pointOfOrigin.localRotation = cachedTransformData;
        return State.Failure;

        void Iterate()
        {
            Debug.DrawRay(m_pointOfOrigin.position, m_pointOfOrigin.forward * m_range);
            m_pointOfOrigin.Rotate(0f, rotationPerIteration, 0f, Space.Self);
        }
    }
}
