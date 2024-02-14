using UnityEngine;

namespace GameAI.BehaviorSystem
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Transform m_pivot;
        [SerializeField] private float m_speed = 90f;

        private Weapon m_weapon = new Weapon();

        private void Update()
        {
            m_pivot.Rotate(0f, 90f * Time.deltaTime, 0f, Space.Self);
        }

        public Weapon Pickup()
        {
            Destroy(gameObject);
            return m_weapon;
        }
    }
}