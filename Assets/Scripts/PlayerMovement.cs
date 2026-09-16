using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;

    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float fallMultiplier = 2.5f;
    bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight;

    private PlayerStamina playerStamina;

    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    float moveSpeed;

    Vector3 moveDirection;

    Rigidbody playerRigidBody;
    Animator animator;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
        playerStamina = GetComponent<PlayerStamina>();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.3f, Vector3.down, playerHeight * 0.5f + 0.5f, whatIsGround);
        Debug.DrawRay(transform.position + Vector3.up * 0.3f, Vector3.down * (playerHeight * 0.5f + 0.5f), grounded ? Color.green : Color.red);
        MyInput();
        SpeedControl();

        if (grounded)
            playerRigidBody.linearDamping = groundDrag;
        else
            playerRigidBody.linearDamping = 0;


        if (animator != null)
        {
            float speed = new Vector3(playerRigidBody.linearVelocity.x, 0f, playerRigidBody.linearVelocity.z).magnitude;
            animator.SetFloat("Speed", speed);

            Vector3 localVelocity = transform.InverseTransformDirection(playerRigidBody.linearVelocity);
            animator.SetFloat("VelocityX", localVelocity.x);
            animator.SetFloat("VelocityZ", localVelocity.z);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
        playerRigidBody.MoveRotation(Quaternion.Euler(0, orientation.eulerAngles.y, 0));

        if (!grounded && playerRigidBody.linearVelocity.y < 0)
            playerRigidBody.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && playerStamina.CanRun())
            moveSpeed = runSpeed;
        else
            moveSpeed = walkSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && grounded && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (animator != null)
            animator.SetBool("IsJumping", !grounded);
    }

    private void Jump()
    {
        playerRigidBody.linearVelocity = new Vector3(playerRigidBody.linearVelocity.x, 0f, playerRigidBody.linearVelocity.z);
        playerRigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        playerRigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRigidBody.linearVelocity.x, 0f, playerRigidBody.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerRigidBody.linearVelocity = new Vector3(limitedVel.x, playerRigidBody.linearVelocity.y, limitedVel.z);
        }
    }
}
