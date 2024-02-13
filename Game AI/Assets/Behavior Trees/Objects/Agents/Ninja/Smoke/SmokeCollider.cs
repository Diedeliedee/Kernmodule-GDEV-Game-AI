using System.Collections.Generic;
using UnityEngine;

public class SmokeCollider : MonoBehaviour
{
    private List<ISmokeInteractable> m_envelopedObjects = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ISmokeInteractable _interactable)) return;
        m_envelopedObjects.Add(_interactable);
        _interactable.OnSmokeEnter();
    }

    public void Tick(float _smokeDensity)
    {
        foreach (var envelopedObject in m_envelopedObjects)
        {
            envelopedObject.OnSmokeStay(_smokeDensity);
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ISmokeInteractable _interactable)) return;
        m_envelopedObjects.Remove(_interactable);
        _interactable.OnSmokeExit();
    }

    private void OnDestroy()
    {
        foreach (var _interactable in m_envelopedObjects) _interactable.OnSmokeExit();
        m_envelopedObjects.Clear();
    }
}
