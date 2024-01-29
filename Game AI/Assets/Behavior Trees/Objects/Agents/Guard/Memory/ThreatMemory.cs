using UnityEngine;

public class ThreatMemory
{
    public Transform threat = null;
    public Vector3 lastSeenThreatLocation = default;
    public Vector3 locationPrediction = default;

    public bool hasSeenThreat => threat != null;

    public void RegisterThreat(Transform _threat, Vector3 _velocity, float _aheadTime)
    {
        threat = _threat;
        lastSeenThreatLocation = _threat.position;
        locationPrediction = lastSeenThreatLocation + _velocity * _aheadTime;
    }

    public void Forget()
    {
        threat = null;
        lastSeenThreatLocation = default;
    }
}
