using UnityEngine.InputSystem;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float sensX;
    public float sensY;

    public Transform orientation;

    [Header("View Toggle")]
    public KeyCode toggleViewKey = KeyCode.V;
    public float thirdPersonDistance = 4f;

    float xRotation;
    float yRotation;

    bool isFirstPerson = true;
    Camera cam;
    int playerBodyMask;
    int obstructionMask;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = GetComponent<Camera>();
        playerBodyMask = 1 << LayerMask.NameToLayer("PlayerBody");
        obstructionMask = ~(playerBodyMask | (1 << LayerMask.NameToLayer("Ignore Raycast")));
    }

    void Update() {

        float mouseX = Input.GetAxis("Mouse X") * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (Input.GetKeyDown(toggleViewKey))
            ToggleView();

        transform.position = isFirstPerson
            ? transform.parent.position
            : ThirdPersonPosition();
    }

    Vector3 ThirdPersonPosition() {
        Vector3 pivot = transform.parent.position;
        Vector3 direction = -transform.forward;
        float distance = thirdPersonDistance;

        if (Physics.Raycast(pivot, direction, out RaycastHit hit, distance, obstructionMask))
            distance = Mathf.Max(hit.distance - 0.2f, 0.1f);

        return pivot + direction * distance;
    }

    public void SetYaw(float degrees) {
        yRotation = degrees;
    }

    void ToggleView() {
        isFirstPerson = !isFirstPerson;

        if (isFirstPerson)
            cam.cullingMask &= ~playerBodyMask;
        else
            cam.cullingMask |= playerBodyMask;
    }
}
