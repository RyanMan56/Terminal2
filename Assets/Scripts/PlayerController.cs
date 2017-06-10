using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private float speed = 6.0f;
    private float mouseSensitivity = 2.0f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float verticalVelocity = 0f;
    private float verticalRotation = 0;
    private float jumpSpeed = 5.0f;
    public bool canMove;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        float yAxisRot = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, yAxisRot, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (canMove)
        {
            if (controller.isGrounded && Input.GetButton("Jump"))
                verticalVelocity = jumpSpeed;

            moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, verticalVelocity, Input.GetAxis("Vertical") * speed);
            moveDirection = transform.TransformDirection(moveDirection);
            //        moveDirection *= speed;

            controller.Move(moveDirection * Time.deltaTime);
        }
	}

    public float pushPower = 2.0F;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}
