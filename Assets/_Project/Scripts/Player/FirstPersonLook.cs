using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using Vanguard;

public class FirstPersonLook : NetworkBehaviour
{
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    
    [HideInInspector]
    public float xRotation = 0.0f;

    [HideInInspector]
    public float yRotation = 0.0f;
    private Vector2 inputVector;
    private bool lookEnabled = true;

    [SerializeField]
    public Camera cam;
    public float targetDutch;
    public float targetHeight = 1;

    // TODO: This should be put somewhere more logical where it can be accessed by
    //    both FirstPersonLook.cs and FirstPersonMove.cs
    public PilotActionControls pilotActionControls;

    private void Awake()
    {
        pilotActionControls = new PilotActionControls();
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            cam.enabled = false;
            pilotActionControls.Disable();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnEnable()
    {
        pilotActionControls?.Enable();
    }

    private void OnDisable()
    {
        pilotActionControls?.Disable();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (lookEnabled)
        {
            inputVector = pilotActionControls.VanguardPilot.Mouse.ReadValue<Vector2>();

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
        }
        cam.transform.localPosition = Vector3.Lerp(
            cam.transform.localPosition,
            new Vector3(0.0f, targetHeight, 0.0f),
            Time.deltaTime * 10
        );
    }

    public void SetLookEnabled(bool enabled) {
        if (lookEnabled != enabled) {
            if (enabled) {
                Cursor.lockState = CursorLockMode.None;
                //xRotation = cam.transform.eulerAngles.x;
                //yRotation = cam.transform.eulerAngles.y;
            }
            else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        lookEnabled = enabled;
    }
}
