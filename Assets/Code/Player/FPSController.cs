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
    [SerializeField]
    private float fallR;
    [SerializeField]
    private float movementLRSpeed = 4;
    [SerializeField]
    private float recoveryLock = 0;
    [SerializeField]
    private float recoveryTime = 2f;

    bool _highJump = false;

    bool jumpActive = false;
    bool _recover = false;
    bool jumpRecover = false;

    bool check = false;

    bool moving = false;

    private float gravityMultiplier;

    private bool sprintKey;

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
        sprintKey = false;
        sprinting = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (PauseMenu.IsOn)
            return;

        Movement();

        Sprint();

        SprintCheck();

        MovementCheck();

        SideStep();

        SprintJumpCheck();

        FallRecovery();

        if (Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            if (sprinting == true)
            {
                sprinting = false;
                movementSpeed = movementSpeed - speedMultiplier;
                fallR = fallR + 0.7f;
                shootPoint.transform.localPosition = new Vector3(0, -0.1f, baseRange);
            }
        }

        ApplyGravity();

    }

    #region Movement Mechanics
    void SprintCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            sprintKey = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            sprintKey = false;
    }

    void Sprint()
    {
        if (sprintKey == true && sprinting == false && player.isGrounded == true)
        {
            movementSpeed = movementSpeed + speedMultiplier;
            fallR = fallR - 0.7f;
            shootPoint.transform.localPosition = new Vector3(0, -0.1f, addRange);
            sprinting = true;
        }

        if (sprintKey == false && sprinting == true)
        {
            movementSpeed = movementSpeed - speedMultiplier;
            fallR = fallR + 0.7f;
            shootPoint.transform.localPosition = new Vector3(0, -0.1f, baseRange);
            sprinting = false;
        }
    }

    void Movement()
    {
        moveFB = Input.GetAxisRaw("Vertical") * movementSpeed;
        moveLR = Input.GetAxisRaw("Horizontal") * movementLRSpeed;
        rotX = Input.GetAxisRaw("Mouse X") * sensitivity;
        rotY -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -rotateLimit, rotateLimit);

        Vector3 movement = new Vector3(moveLR, vertVelocity, moveFB);
        transform.Rotate(0, rotX, 0);
        cam.transform.localRotation = Quaternion.Euler(rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);
    }

    void MovementCheck()
    {
        if (Input.GetKeyDown(KeyCode.W) && moving == false)
        {
            moving = true;
        } else if (Input.GetKeyUp(KeyCode.W) && moving == true)
        {
            moving = false;
        }
        if (Input.GetKeyDown(KeyCode.S) && moving == false)
        {
            moving = true;
        } else if (Input.GetKeyUp(KeyCode.S) && moving == true)
        {
            moving = false;
        }

    }

    void SideStep()
    {
        if (moving == true && check == false)
        {
            movementLRSpeed = movementLRSpeed - speedMultiplier;
            check = true;
        }

        if (moving == false && check == true)
        {
            movementLRSpeed = movementLRSpeed + speedMultiplier;
            check = false;
        }
    }
    #endregion

    #region Jumping Mechanics
    void JumpReset()
    {
        jumpForce = jumpForce - jumpMultiplier;
        _highJump = false;
    }

    public void HighJump()
    {

        jumpForce = jumpForce + jumpMultiplier;
        _highJump = true;
        Invoke("JumpReset", 10f); 
    }

    private void ApplyGravity()
    {
        if (player.isGrounded == true)
        {
            if (hasJumped == false)
            {
                vertVelocity = Physics.gravity.y;
                gravityMultiplier = fallR;
            }
            else
            {
                vertVelocity = jumpForce;
            }
        }
        else
        {
            gravityMultiplier += Time.deltaTime;
            vertVelocity += Physics.gravity.y * Time.deltaTime * gravityMultiplier;
            vertVelocity = Mathf.Clamp(vertVelocity, -999999, jumpForce);
            hasJumped = false;
        }
    }
    #endregion

    #region Fall Recovery

    void SprintJumpCheck()
    {
        if (sprintKey == true && player.isGrounded == true)
        {
            jumpActive = true;
        }

        if (sprintKey == false && player.isGrounded == true)
        {
            jumpActive = false;
        }
    }

    void FallRecovery()
    {
        if (_highJump == false && recoveryLock < gravityMultiplier && jumpRecover == false && jumpActive == true)
        {
            _recover = true;
        }

        if (_recover == true && player.isGrounded == true)
        {
            movementSpeed = movementSpeed - jumpMultiplier;
            movementLRSpeed = movementLRSpeed - speedMultiplier;
            jumpForce = jumpForce - speedMultiplier;
            Invoke("RecoveryReset", recoveryTime);
            jumpRecover = true;
            _recover = false;
        }
    }

    void RecoveryReset()
    {
        movementSpeed = movementSpeed + jumpMultiplier;
        movementLRSpeed = movementLRSpeed + speedMultiplier;
        jumpForce = jumpForce + speedMultiplier;
        jumpRecover = false;
    }



    #endregion
}
