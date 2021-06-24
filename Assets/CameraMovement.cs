using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lookSensitivity = 3f;
    [SerializeField] GameObject fpsCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float CameraUpAndDownRotation = 0f;
    private float CurrentCameraUpAndDownRotation = 0f;
    private Rigidbody rb;

    private void Start()
    {
      Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 movementHorizontal = transform.right * Input.GetAxis("Horizontal");
        Vector3 movementVertical = transform.forward * Input.GetAxis("Vertical");
        Vector3 movementVelocity = (movementHorizontal + movementVertical).normalized * speed;
        Move(movementVelocity);
        Rotate();
    }

    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position+velocity*Time.fixedDeltaTime);
        }
        rb.MoveRotation(rb.rotation*Quaternion.Euler(rotation));
        if (fpsCamera!=null)
        {
            CurrentCameraUpAndDownRotation -= CameraUpAndDownRotation;
            CurrentCameraUpAndDownRotation = Mathf.Clamp(CurrentCameraUpAndDownRotation, -85, 85);
            fpsCamera.transform.localEulerAngles = new Vector3(CurrentCameraUpAndDownRotation,0,0);
        }
    }

    private void Move(Vector3 movementVelocity)
    {
        velocity = movementVelocity;
    }

    private void Rotate()
    {
        float yRotation = Input.GetAxis("Mouse X");
        float cameraUpAndDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;
        rotation =  new Vector3(0, yRotation,0) * lookSensitivity;
        CameraUpAndDownRotation = cameraUpAndDownRotation;
    }
}
