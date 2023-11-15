using Godot;
using System;

public class Boid : CharacterBody3D
{
	[Export] private float cohesionRange = 3;
	[Export] private float cohesionForce = 3;
	[Space]
	[Export] private float seperationRange = 3;
	[Export] private float separationForce = 3;
	
	public override tick()
}
