using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private float m_sensitivity = 3f;
    [SerializeField] private float m_pitchRange = 85f;

    [Header("References:")]
    [SerializeField] private Transform m_camera;
    [SerializeField] private Transform m_pivot;

    private float m_pitch;
    private float m_yaw;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_pitch = m_camera.localEulerAngles.x;
        m_yaw = m_pivot.localEulerAngles.y;
    }

    private void Update()
    {
        var iteration = new Vector2 (Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * m_sensitivity;

        m_pitch = Mathf.Clamp(m_pitch - iteration.y, -m_pitchRange, m_pitchRange);
        m_yaw = Mathf.Repeat(m_yaw + iteration.x, 360f);

        m_camera.localEulerAngles = new Vector3(m_pitch, 0f, 0f);
        m_pivot.localEulerAngles = new Vector3(0f, m_yaw, 0f);
    }
}
