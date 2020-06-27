using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;//This will be the speed of the player
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 5f);

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;//Creates new Rigidbody2D component variable called myRigidBody
    Animator myAnimator;
    CapsuleCollider2D  myBodCollider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    //Message the methods

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();//This sets the myRigidBody variable to the RigidBody2D already apart of Player
        myAnimator = GetComponent<Animator>();
        myBodCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");//value is between -1 and +1(left to right)
        //Using CrossPlatformInputManager from Standard Assets allows me to use other stuff like joysticks

        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);//velocity represents the rate of change
        //The x value of this Vector2 is decided upon if Player goes left(-1) or right(+1) and multiplies this by the runSpeed variable
        //The y value of this Vector2 is whatever the Players current RigidBody2D velocity is

        myRigidBody.velocity = playerVelocity;//This controls where the player actually is now based off the playerVelocity variable

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);//So only if player is moving the Running animation is triggered
    }

    private void ClimbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return; 
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", true);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }//if Player isnt touching Ground then it just returns

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;//Everytime player clicks space and is touching the Ground layer he jumps
        }
    }

    private void Die()
    {
        if (myBodCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;

            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FlipSprite()
    {
        //if player is moving horrizontally
        //then reverse scaling of x axis

        //Mathf.Sign returns a value of -1 or 1
        //Absolute value is the opposite number. So absolute value of -3 is 3

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //If the x velocity of Players myRigidBody is greater than 0 then this is true
        if (playerHasHorizontalSpeed)//If player is moving
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);//the scale of the transform relative to the GameObjects parent is being 
            //x value of the localScale is the Sign(-1 or 1) depending on velocity x value.
            //y value stays at 1
        }


    }
}
