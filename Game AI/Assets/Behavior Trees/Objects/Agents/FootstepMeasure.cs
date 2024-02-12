using Joeri.Tools.Debugging;
using UnityEngine;
using UnityEngine.Events;

public class FootstepMeasure : MonoBehaviour
{
    [SerializeField] private float m_distance = 3f;
    [Space]
    [SerializeField] private UnityEvent m_onStep;

    private Vector3 m_lastLocation = default;

    private void Start()
    {
        m_lastLocation = transform.position;
    }

    private void Update()
    {
        var sqrDistance = (transform.position - m_lastLocation).sqrMagnitude;
        var sqrMeasured = m_distance * m_distance;

        if (sqrDistance > sqrMeasured)
        {
            m_lastLocation = transform.position;
            m_onStep.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        GizmoTools.DrawOutlinedDisc(m_lastLocation, m_distance, Color.cyan, Color.white, 0.25f);
    }
}
