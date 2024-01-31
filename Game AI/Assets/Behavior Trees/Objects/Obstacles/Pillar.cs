using UnityEngine;

public class Pillar : MonoBehaviour, IHidingCover
{
    [SerializeField] private float m_hidingDistance = 0.5f;

    private CapsuleCollider m_collider = null;

    public Vector3 position => transform.position;

    private void Awake()
    {
        m_collider = GetComponentInChildren<CapsuleCollider>();
    }

    public Vector3 GetHidingPosition(Vector3 _from)
    {
        return transform.position + ((transform.position - _from).normalized * (m_collider.radius + m_hidingDistance));
    }
}