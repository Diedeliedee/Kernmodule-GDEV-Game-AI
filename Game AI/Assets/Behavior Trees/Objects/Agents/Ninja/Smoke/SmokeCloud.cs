using Joeri.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCloud : MonoBehaviour
{
    [SerializeField] private float m_time = 10f;
    [SerializeField] private MeshRenderer m_renderer;

    private Timer m_timer = null;

    private void Awake()
    {
        m_timer = new Timer(m_time);
    }

    private void Update()
    {
        if (m_timer.HasReached(Time.deltaTime))
        {
            Destroy(gameObject);
            return;
        }

        var color = m_renderer.material.color;

        color.a = m_timer.GetPercent(true) * -1 + 1;
        m_renderer.material.color = color;
    }
}
