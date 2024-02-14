using Joeri.Tools;
using Joeri.Tools.Utilities;
using UnityEngine;

namespace GameAI.BehaviorSystem
{
    public class SmokebombHandler : MonoBehaviour
    {
        [SerializeField] private float m_velocity = 30f;
        [SerializeField] private float m_cooldown = 1f;
        [SerializeField] private float m_minThrowAngle = 30f;
        [Space]
        [SerializeField] private GameObject m_bombObject;

        private Timer m_timer = null;
        private bool m_hasThrown = false;

        public bool canThrow => !m_hasThrown;

        private void Start()
        {
            m_timer = new Timer(m_cooldown);
        }

        private void Update()
        {
            if (m_hasThrown && m_timer.ResetOnReach(Time.deltaTime)) m_hasThrown = false;
        }

        public void ThrowBombTo(Vector3 _targetPos)
        {
            transform.LookAt(_targetPos, Vector3.up);

            var localEulers = transform.localEulerAngles;
            var distance = Vector2.Distance(transform.position.Planar(), _targetPos.Planar());
            var bomb = Instantiate(m_bombObject, transform.position, transform.rotation).GetComponent<Smokebomb>();

            localEulers.x = -GetLaunchAngle(m_velocity, distance);
            transform.localEulerAngles = localEulers;
            bomb.Launch(transform.forward * m_velocity);
            m_hasThrown = true;

            Debug.Log("Throwing bomb!!!");
        }

        private float GetLaunchAngle(float _velocity, float _range)
        {
            return Mathf.Asin(_range * Physics.gravity.magnitude / (2 * _velocity * _velocity)) * Mathf.Rad2Deg;
        }
    }
}