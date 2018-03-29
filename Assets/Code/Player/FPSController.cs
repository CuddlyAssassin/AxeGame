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
    private float speedMultiplier = 4f;
    [SerializeField]
    private float jumpMultiplier = 4f;
    [SerializeField]
    private float addRange;
    [SerializeField]
    private float baseRange;
    [SerializeField]
    private GameObject shootPoint;

    CharacterController player;

    public GameObject cam;

    float moveFB;
    float moveLR;
    float vertVelocity;
    float rotX;
    float rotY;

    private bool hasJumped;

    bool sprinting;

	// Use this for initialization
	void Start () {
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (PauseMenu.IsOn)
            return;

        Movement();

        Sprint();

        if (Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            if (sprinting == true)
            {
                sprinting = false;
                movementSpeed = movementSpeed - speedMultiplier;
                shootPoint.transform.localPosition = new Vector3(0, -0.1f, baseRange);
            }
        }

        ApplyGravity();

    }

    void JumpReset()
    {
        jumpForce = jumpForce - jumpMultiplier;
    }

    public void HighJump()
    {
        jumpForce = jumpForce + jumpMultiplier;
        Invoke("JumpReset", 10f); 
    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprinting == false && player.isGrounded == true)
        {
            movementSpeed = movementSpeed + speedMultiplier;
            shootPoint.transform.localPosition = new Vector3(0, -0.1f, addRange);
            sprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && sprinting == true)
        {
            movementSpeed = movementSpeed - speedMultiplier;
            shootPoint.transform.localPosition = new Vector3(0, -0.1f, baseRange);
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
            vertVelocity = Mathf.Clamp(vertVelocity, -999999, jumpForce);
            hasJumped = false;
        }
    }
}
