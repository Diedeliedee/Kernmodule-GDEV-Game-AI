using UnityEngine;

public class TargetMemory
{
    private bool m_targetSet = false;
    private Vector3 m_target = default;
    private float m_epsilon = 0f;

    public bool targetSet => m_targetSet;
    public Vector3 target => m_target;
    public float epsilon => m_epsilon;

    public TargetMemory()
    {
        m_epsilon = 0f;
    }

    public TargetMemory(float _epsilon)
    {
        m_epsilon = _epsilon;
    }

    public void SetTarget(Vector3 _target)
    {
        m_targetSet = true;
        m_target = _target;
    }

    public void Reset()
    {
        m_targetSet = false;
        m_target = default;
    }
}
