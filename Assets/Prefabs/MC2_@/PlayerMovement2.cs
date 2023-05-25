using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement2 : MonoBehaviour
{
#region variables
    public float speed;
    public float jumpForce;
    public Transform ceilingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects; //Ground Objects
    public float checkRadius;

    private bool allowedMoving = true;

    private bool isJumping = false;
    private Rigidbody2D rigidBody2D;
    private Vector2 moveVelocity;
    private float moveInput;
    private bool facingRight = false;
    private bool isGrounded;
    [SerializeField] private GameObject sprite;

    private Animator animator;
#endregion variables
    
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if(!allowedMoving)
        moveInput = 0;
            rigidBody2D.velocity= new Vector2(moveInput * speed, rigidBody2D.velocity.y);
        //Check if Player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);   
    }

#region PLAYER_CONTROLS
    public void Move(InputAction.CallbackContext context)
    {
        if(allowedMoving)
        {
                    moveInput = context.ReadValue<Vector2>().x;
        //Animate
        Animate();
        //Animator
        animator.SetFloat("xVelocity", Mathf.Abs(moveInput));
        }

    }
#endregion PLAYER_CONTROLS

#region PLAYER_ANIMATION
    private void Animate()
    {
        if (moveInput > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveInput < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
#endregion PLAYER_ANIMATION
 
#region MOVEMENT_FUNCTIONS 
    public void DisableMovement(){
           animator.SetFloat("xVelocity", 0);
            allowedMoving = false;
    }

    public void EnableMovement(){
        allowedMoving = true;
    }
#endregion MOVEMENT_FUNCTIONS    

#region EVENT_LISTENERS
// Actions

    private UnityAction enableWalk;
    private UnityAction disableWalk;
    
    private void Awake() 
    {
        enableWalk = new UnityAction(EnableMovement);
        disableWalk = new UnityAction(DisableMovement);
    }
    private void OnEnable() 
    {
        EventManager.StartListening("PuzzleInteracted", disableWalk);
        EventManager.StartListening("PuzzleExited", enableWalk);
        EventManager.StartListening("GamePaused", disableWalk);
        EventManager.StartListening("GameResumed", enableWalk);
    }

    private void OnDisable()
    {
        EventManager.StopListening("PuzzleInteracted", disableWalk);
        EventManager.StopListening("PuzzleExited", enableWalk);
        EventManager.StopListening("GamePaused", disableWalk);
        EventManager.StopListening("GameResumed", enableWalk);
    }

#endregion EVENT_LISTENERS
    
}
