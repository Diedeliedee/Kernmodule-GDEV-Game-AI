﻿using Joeri.Tools.Movement;
using Joeri.Tools.Utilities;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float moveGrip = 3;

    [Header("Reference:")]
    [SerializeField] private Transform m_camera;

    private Accel.Flat m_flatAcceleration = new();

    public Vector3 velocity => new Vector3(m_flatAcceleration.velocity.x, 0f, m_flatAcceleration.velocity.y);

    // Update is called once per frame
    private void Update()
    {
        var deltaTime = Time.deltaTime;
        var vert = Input.GetAxisRaw("Vertical");
        var hor = Input.GetAxisRaw("Horizontal");

        Vector3 forwardDirection = Vector3.Scale(new Vector3(1, 0, 1), m_camera.transform.forward);
        Vector3 rightDirection = Vector3.Cross(Vector3.up, forwardDirection.normalized);

        var moveDirection = forwardDirection.normalized * vert + rightDirection.normalized * hor;
        transform.position += Vectors.FlatToVector(m_flatAcceleration.CalculateVelocity(Vectors.VectorToFlat(moveDirection.normalized) * moveSpeed, moveGrip, deltaTime) * deltaTime);
    }
}