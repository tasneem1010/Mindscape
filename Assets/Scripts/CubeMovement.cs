using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 2f;   // Wobble speed
    public float distance = 1f; // How far it wobbles

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store starting position
    }

    void Update()
    {
        // Wobble the cube left and right
        float wobble = Mathf.Sin(Time.time * speed) * distance;

        // Update cube's position (only X axis for a side-to-side effect)
        transform.position = startPosition + new Vector3(wobble, 0, 0);
    }
}
