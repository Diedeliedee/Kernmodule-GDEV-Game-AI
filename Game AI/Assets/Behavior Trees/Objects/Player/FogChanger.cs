using UnityEngine;

public class FogChanger : MonoBehaviour, ISmokeInteractable
{
    [SerializeField] private float m_smokeDensity = 0.4f;

    private float m_defaultFog = 0f;

    private void Start()
    {
        m_defaultFog = RenderSettings.fogDensity;
    }

    public void OnSmokeEnter()
    {
        RenderSettings.fogDensity = m_smokeDensity;
    }

    public void OnSmokeStay(float _smokeDensity)
    {
        RenderSettings.fogDensity = Mathf.Lerp(m_defaultFog, m_smokeDensity, _smokeDensity);
    }

    public void OnSmokeExit()
    {
        RenderSettings.fogDensity = m_defaultFog;
    }

}
