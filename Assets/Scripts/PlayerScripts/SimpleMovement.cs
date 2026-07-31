using UnityEngine.InputSystem;
using UnityEngine;


public class SimpleMovement : MonoBehaviour
{
    public CharacterController charCont;

    public float currentPlayerSpeed;
    public float playerSpeed = 15f;
    public float airDrag = 0.5f;
    private float myGravity = -10f;
    public float jumpH = 2f;

    public float crouchHeightMult = 0.5f;
    public float crouchSpeedMult = 0.7f;

    public Transform groundCheck;
    private float groundDist = 0.4f;
    public LayerMask groundMask;
    private bool onGround;

    private float currentPlayerHeight;
    public bool canMove = true;

    private Vector3 movementVector;
    private Vector3 myVelocity;
    private Vector3 externalVelocity;

    private bool isExternalLaunchActive = false;

    //Input System
    public InputAction playerMove, playerCrouch, playerJump;


    private void OnEnable()
    {
        playerMove = InputSystem.actions.FindAction("Move");
        playerCrouch = InputSystem.actions.FindAction("Crouch");
        playerJump = InputSystem.actions.FindAction("Jump");
    }

    void Start()
    {

        currentPlayerSpeed = playerSpeed;
        currentPlayerHeight = charCont.height;

    }

    void Update()
    {
        if (canMove)
        {
            GetInput();
        }

    }

    void GetInput()
    {
        onGround = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (onGround)
        {
            if (isExternalLaunchActive && myVelocity.y <= 0)
            {
                externalVelocity = Vector3.zero;
                isExternalLaunchActive = false;
            }

            if (myVelocity.y < 0)
            {
                myVelocity.y = -5f;
                playerSpeed = currentPlayerSpeed;
            }
        }
        else
        {
            if (externalVelocity != Vector3.zero)
            {
                isExternalLaunchActive = true;
            }
        }

        Vector2 tempVec = playerMove.ReadValue<Vector2>();

        movementVector = (tempVec.x * transform.right) + (tempVec.y * transform.forward);

        if (playerCrouch.IsPressed())
        {
            charCont.height = currentPlayerHeight * crouchHeightMult;
            playerSpeed = currentPlayerSpeed * crouchSpeedMult;
        }
        else
        {
            charCont.height = currentPlayerHeight;
            playerSpeed = currentPlayerSpeed;
        }

        if (playerJump.WasPressedThisFrame() && onGround)
        {
            myVelocity.y = Mathf.Sqrt(jumpH * -2f * myGravity);
            playerSpeed *= airDrag;
        }

        myVelocity.y += myGravity * Time.unscaledDeltaTime;
        charCont.Move((myVelocity + movementVector * playerSpeed  + externalVelocity ) * Time.unscaledDeltaTime);
        
    }

    public void ApplyExternalVelocity(Vector3 velocity)
    {
        externalVelocity += velocity;

        if(Mathf.Abs(externalVelocity.y) > 0)
            myVelocity.y = Mathf.Max(0f, myVelocity.y);
    }

}