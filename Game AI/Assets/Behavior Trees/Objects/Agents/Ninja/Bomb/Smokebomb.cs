using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Smokebomb : MonoBehaviour
{
    [SerializeField] private GameObject m_smokeCloud;

    private Rigidbody m_rb = null;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 _velocity)
    {
        m_rb.velocity = _velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Instantiate(m_smokeCloud, transform.position, Quaternion.identity);
    }
}
