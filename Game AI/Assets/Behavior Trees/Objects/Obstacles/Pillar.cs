using UnityEngine;

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
        _normal = (transform.position - _from).normalized;
        return transform.position + _normal * m_collider.radius;
    }
}