using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTester : MonoBehaviour
{
    [SerializeField] private SmokebombHandler m_bomb;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        m_bomb.ThrowBombTo(transform.position);
    }
}
