using Joeri.Tools.Debugging;
using UnityEngine;

public class ThreatPerception
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
        GizmoTools.DrawOutlinedDisc(lastSeenThreatLocation, 0.5f, Color.white, Color.white, 0.25f);
        GizmoTools.DrawOutlinedDisc(locationPrediction, 0.5f, Color.green, Color.white, 0.5f);
    }
}
