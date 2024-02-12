using UnityEngine;

public class ColliderOcclusionCheck : MonoBehaviour
{
    [SerializeField] private Collider m_target;
    [Space]
    [SerializeField] private float m_angle = 45f;
    [SerializeField] private float m_distance = 10f;
    [SerializeField] private LayerMask m_occluders = default;

    public bool CanReachTarget(out Vector3 _rayEndPoint)
    {
        var target = m_target.ClosestPoint(transform.position);
        var distance = Vector3.Distance(transform.position, target);
        var direction = (target - transform.position).normalized;

        _rayEndPoint = transform.position + direction * distance;
        if (distance > m_distance)
        {
            _rayEndPoint = transform.position + direction * m_distance;
            return false;
        }

        var hitOccluder = Physics.Raycast(transform.position, direction, out RaycastHit _hit, distance, m_occluders, QueryTriggerInteraction.Collide);

        if (hitOccluder)
        {
            _rayEndPoint = _hit.point;
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = CanReachTarget(out Vector3 _rayEndPoint) ? Color.white : Color.red;
        Gizmos.DrawLine(transform.position, _rayEndPoint);
    }
}