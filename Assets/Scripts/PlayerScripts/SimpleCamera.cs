using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCamera : MonoBehaviour
{

    public float sensitivity = 100f;

    public Transform body;
    //public Transform cam;

    public InputAction lookMove;

    private float xRot = 0f;

    private void OnEnable()
    {
        lookMove = InputSystem.actions.FindAction("Look");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 looking = lookMove.ReadValue<Vector2>() * sensitivity * Time.deltaTime;

        xRot -= looking.y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        body.Rotate(Vector3.up * looking.x);
    }

}