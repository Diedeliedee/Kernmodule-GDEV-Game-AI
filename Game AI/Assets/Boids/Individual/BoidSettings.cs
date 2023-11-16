using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Boid Settings", menuName = "Settings/Boid Settings", order = 1)]
public class BoidSettings : ScriptableObject
{
    public float travelSpeed = 10;
    [Space]
    public float cohesionForce = 3;
    public float seperationForce = 3;
    public float alignmentForce = 3;
    [Space]
    public float boundaryOpposingForce = 3;
    [Space]
    public float grip = 3;
    [Space]
    public float cohesionDistance = 3;
    public float seperationDistance = 3;
}
