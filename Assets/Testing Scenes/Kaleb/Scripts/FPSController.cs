using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = 2f;
    [SerializeField]
    private float sensitivity = 2f;
    [SerializeField]
    private float rotateLimit = 85f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float multiplier = 4f;


    CharacterController player;

    public GameObject cam;

    float moveFB;
    float moveLR;
    float vertVelocity;
    float rotX;
    float rotY;

    private bool hasJumped;

    bool sprinting;
    bool locked;

	// Use this for initialization
	void Start () {
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        locked = true;
    }
	
	// Update is called once per frame
	void Update () {

        Movement();

        Sprint();

        MouseLock();

        if (Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
        }

        ApplyGravity();

    }
    
    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprinting == false)
        {
            movementSpeed = movementSpeed + multiplier;
            sprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && sprinting == true)
        {
            movementSpeed = movementSpeed - multiplier;
            sprinting = false;
        }
    }

    void Movement()
    {
        moveFB = Input.GetAxisRaw("Vertical") * movementSpeed;
        moveLR = Input.GetAxisRaw("Horizontal") * movementSpeed;
        rotX = Input.GetAxisRaw("Mouse X") * sensitivity;
        rotY -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -rotateLimit, rotateLimit);

        Vector3 movement = new Vector3(moveLR, vertVelocity, moveFB);
        transform.Rotate(0, rotX, 0);
        cam.transform.localRotation = Quaternion.Euler(rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (player.isGrounded == true)
        {
            if (hasJumped == false)
            {
                vertVelocity = Physics.gravity.y;
            }
            else
            {
                vertVelocity = jumpForce;
            }
        }
        else
        {
            vertVelocity += Physics.gravity.y * Time.deltaTime;
            vertVelocity = Mathf.Clamp(vertVelocity, -50, jumpForce);
            hasJumped = false;
        }
    }

    void MouseLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && locked == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            locked = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && locked == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            locked = true;
        }
    }
}
