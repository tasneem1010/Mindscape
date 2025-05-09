using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 80f; 
    public Transform playerBody; 

    private float xRotation = 0f;
    private FixedJoystick lookJoystick;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        var joystick = GameObject.Find("Look Joystick");
        if (joystick != null)
        {
            lookJoystick = joystick.GetComponent<FixedJoystick>();

            Cursor.visible = true; // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        if (lookJoystick != null && lookJoystick.gameObject.activeInHierarchy)
        {
            mouseX = lookJoystick.Horizontal;
            mouseY = lookJoystick.Vertical;
        }
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
