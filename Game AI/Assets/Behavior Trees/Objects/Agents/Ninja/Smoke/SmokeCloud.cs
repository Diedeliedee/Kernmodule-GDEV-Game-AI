using Joeri.Tools;
using Joeri.Tools.Utilities;
using UnityEngine;

namespace GameAI.BehaviorSystem
{
    public class SmokeCloud : MonoBehaviour
    {
        [SerializeField] private float m_time = 10f;

        private MeshRenderer[] m_renderers;
        private SmokeCollider m_collider = null;
        private Timer m_timer = null;

        private void Awake()
        {
            m_renderers = GetComponentsInChildren<MeshRenderer>(true);
            m_collider = GetComponentInChildren<SmokeCollider>(true);
            m_timer = new Timer(m_time);
        }

        private void Update()
        {
            m_collider.Tick(Util.OneMinus(m_timer.percent));

            if (m_timer.HasReached(Time.deltaTime))
            {
                Destroy(gameObject);
                return;
            }

            var alpha = m_timer.GetPercent(true) * -1 + 1;

            foreach (var _renderer in m_renderers)
            {
                var color = _renderer.material.color;

                color.a = alpha;
                _renderer.material.color = color;
            }
        }
    }
}