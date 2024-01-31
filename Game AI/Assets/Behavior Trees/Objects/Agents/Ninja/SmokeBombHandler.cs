using Joeri.Tools;
using UnityEngine;

public class SmokebombHandler : MonoBehaviour
{
    [SerializeField] private float m_cooldown = 1f;

    private Timer m_timer = null;
    private bool m_hasThrown = false;

    public bool canThrow => !m_hasThrown;

    private void Start()
    {
        m_timer = new Timer(1f);
    }

    private void Update()
    {
        if (m_hasThrown && m_timer.ResetOnReach(Time.deltaTime)) m_hasThrown = false;
    }

    public void ThrowBombTo(Vector3 _targetPos)
    {
        Debug.Log("Throwing bomb!!!");
        m_hasThrown = true;
    }
}