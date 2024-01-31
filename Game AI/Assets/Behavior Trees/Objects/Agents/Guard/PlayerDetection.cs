using UnityEngine;

[RequireComponent(typeof(Light))]
public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private LayerMask m_mask = default;
    [SerializeField] private int m_accuracy = 10;
    [SerializeField] private bool m_debugMode = false;

    private Light m_light = null;

    private void Awake()
    {
        m_light = GetComponent<Light>();
    }

    public bool PlayerDetected()
    {
        var originalRotation = transform.localRotation;
        var offset = m_light.spotAngle / 2f;

        for (int i = 0; i < m_accuracy; i++)
        {
            var randomOffset = RandomOffset() * offset;

            //  Apply random offset to euler angles of camera.
            transform.Rotate(randomOffset.x, randomOffset.y, 0f, Space.Self);

            //  Cast a ray, and continue to the next iteration if neither anything has been hit, or whatever's been hit isn't a player.
            if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit _hit, m_light.range, m_mask))
            {
                if (m_debugMode) Debug.DrawRay(transform.position, transform.forward * m_light.range);
                transform.localRotation = originalRotation;
                continue;
            }

            //  Draw the hit ray (optional), and reset rotation.
            if (m_debugMode) Debug.DrawLine(transform.position, _hit.point);
            transform.localRotation = originalRotation;

            //  Return true if the player has been found.
            if (_hit.transform.TryGetComponent(out PlayerMovement _player))
            {
                if (m_debugMode) Debug.Log("Player detected", gameObject);
                return true;
            }
        }
        return false;
    }

    private Vector2 RandomOffset()
    {
        var offset = Vector2.zero;

        for (int i = 0; i < m_accuracy; i++)
        {
            offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            if (offset.sqrMagnitude > 1f) continue;
            break;
        }
        return offset;
    }
}