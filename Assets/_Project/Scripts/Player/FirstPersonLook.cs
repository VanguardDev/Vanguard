using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private Vector2 inputVector;
    private bool lookEnabled = true;

    public Camera cam;
    public float targetDutch;
    public float targetHeight = 1;
    
    public void UpdateInput(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Cursor.visible = false;

        float mouseX = inputVector.x * horizontalSpeed;
        float mouseY = inputVector.y * verticalSpeed;
    
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, 0.0f));
        cam.transform.rotation = Quaternion.Lerp(
            Quaternion.Euler(new Vector3(xRotation, yRotation, cam.transform.eulerAngles.z)), 
            Quaternion.Euler(new Vector3(xRotation, yRotation, targetDutch)), 
            Time.deltaTime * 10
        );

        cam.transform.localPosition = Vector3.Lerp(
            cam.transform.localPosition,
            new Vector3(0.0f, targetHeight, 0.0f),
            Time.deltaTime * 10
        );
    }

    public void SetLookEnabled(bool enabled) {
        if (lookEnabled != enabled) {
            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
        }
        lookEnabled = enabled;
    }
}
