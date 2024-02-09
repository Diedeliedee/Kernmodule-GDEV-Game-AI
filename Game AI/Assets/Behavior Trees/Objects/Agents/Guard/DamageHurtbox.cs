using UnityEngine;

public class DamageHurtbox : MonoBehaviour
{
    [SerializeField] private int m_damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamagable _damagable)) return;
        _damagable.Damage(m_damage);
    }
}
