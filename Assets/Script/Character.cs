using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    public float runSpeed, walkSpeed;
    public float jumpForce = 5f;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody2D rg;
    [HideInInspector]
    public float movX,movY;

    // char animation states
    public const int stateIdle = 0;
    public const int stateIdleBlinking = 1;
    public const int stateWalk = 2;
    public const int stateRun = 3;
    public const int stateSlide = 4;
    public const int stateJump = 5;

    [HideInInspector]
    public bool isMoving, isJumping, isRunning,isSliding;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveCharacter();
    }

    void FixedUpdate()
    {
        CharacterAnimations();
    }

    public void moveCharacter()
    {
        speed = walkSpeed;
        if (Input.GetKeyDown(KeyCode.C)&&!isJumping)
        {
            print("jumped");
            rg.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;

        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            speed = runSpeed;
        }
        else
        {
            isRunning = false;
            isJumping = false;
        }
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");
        rg.velocity= new Vector3(movX, movY, 0f) * speed;

    }

    public void AnimateMov()
    {
        Vector3 dir  = new(0.2f, 0.2f, 0.2f);
        isMoving = false;
        if (movX > 0)
        {
            // moving right
            dir.x = 0.2f;

        }
        else if (movX < 0)
        {
            // moving left
            dir.x = -0.2f;
        }

        //else if(movY > 0)
        //{
        //    // moving up

        //}else if (movY < 0)
        //{
        //    // moving down

        //}
        if (movX != 0 || movY != 0)
        {
            // moving
            isMoving = true;
            transform.localScale = dir;
            SetAnimation(stateWalk);
            //print("moving");
        }
    }


    public void AnimateIdle()
    {
        if (!isMoving)
        {
            SetAnimation(stateIdle);
        }
    }

    public void AnimateJump()
    {
        if (isJumping)
        {
            SetAnimation(stateJump);
        }
    }

    public void AnimateRun()
    {
        if (isRunning && isMoving)
        {
            print("running");
            SetAnimation(stateRun);
        }
    }

    public void CharacterAnimations()
    {
        AnimateMov();
        AnimateIdle();
        AnimateJump();
        AnimateRun();
    }

    public void SetAnimation(int state)
    {
        animator.SetInteger("charState", state);
    }
}
