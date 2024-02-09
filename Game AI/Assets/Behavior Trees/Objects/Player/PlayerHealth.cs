using Joeri.Tools.Gameify;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private Health m_health;
    [Space]
    [SerializeField] private UnityEvent<int, int> m_onDamage;
    [SerializeField] private UnityEvent m_onDeath;

    private Animator m_animator = null;

    private void Awake()
    {
        m_health.onHealthChange += (h, m) => m_onDamage.Invoke(h, m);
        m_health.onDeath += () => m_onDeath.Invoke();

        m_animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        //  Not sure if this is techincally needed, but just to be safe.
        m_health.onHealthChange = null;
        m_health.onDeath = null;
    }

    public void Damage(int _damage)
    {
        m_health.ChangeHealth(-_damage);
    }
}
