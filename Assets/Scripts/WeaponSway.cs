using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth = 8f;
    [SerializeField] private float multiplier = 2f;

    private Quaternion initialRotation;

    private void Start()
    {
        // Store the initial rotation of the weapon
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        // Calculate target rotation based on mouse input
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        // Combine the initial rotation with the calculated rotation
        Quaternion targetRotation = initialRotation * rotationX * rotationY;

        // Smoothly interpolate to the target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
