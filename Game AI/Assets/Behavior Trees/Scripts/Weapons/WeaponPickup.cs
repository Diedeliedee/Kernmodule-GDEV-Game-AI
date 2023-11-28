using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private Weapon m_weapon = new Weapon();

    public Weapon Pickup()
    {
        Destroy(gameObject);
        return m_weapon;
    }
}
