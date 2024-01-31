using UnityEngine;

public class ThreatMemory
{
    public Vector3 lastSeenThreatLocation = default;
    public Vector3 locationPrediction = default;

    public bool hasSeenThreat { get; private set; }

    public void RegisterThreat()
    {
        hasSeenThreat = true;
    }

    public void UpdateThreatInfo(Transform _threat, Vector3 _velocity, float _aheadTime)
    {
        lastSeenThreatLocation = _threat.position;
        locationPrediction = lastSeenThreatLocation + _velocity * _aheadTime;
    }

    public void Forget()
    {
        hasSeenThreat = false;
        lastSeenThreatLocation = default;
        locationPrediction = default;
    }

    public void Draw()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(lastSeenThreatLocation, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(locationPrediction, 0.5f);
    }
}
