using System;
using System.Collections;
using UnityEngine;

public class Hunter : Character
{
    // VARIABLES
    // movements
    bool isAttacking = false;

    // char animation states
    public const int stateAttackSlash = 6;
    public const int stateAttckKick = 7;

    // FUNCTIONS
    // public Hunter() { }

    void Start()
    {
        animator = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        cmdMoveCharacter();
        moveHunter();
        AnimateBasicCharacterMovs();
        AnimateHunterCharacterMovs();
        CmdCharacterAnimations(currentAnimateState);
    }

    // MOVEMENTS
    public void moveHunter()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            print("attacking");
            isAttacking = true;
        }
        else if (Input.GetKeyDown(KeyCode.X) && isMoving && !isSliding)
        {
            print("start sliding");
            isSliding = true;
            slideSecs = maxSlideSecs;
            slideX = Input.GetAxisRaw("Horizontal");
            slideY = Input.GetAxisRaw("Vertical");
        }
        else
        {
            isAttacking = false;
        }
    }

    // ANIMATIONS
    void AnimateAttackSlash()
    {
        if (isAttacking)
        {
            currentAnimateState = stateAttackSlash;
        }
    }

    void AnimateSlide()
    {
        if (isSliding)
        {
            rg.velocity = new Vector2(slideX, slideY) * runSpeed;
            // print(slideX);
            // print(slideSecs);
            // SetAnimation(stateSlide);
            currentAnimateState = stateSlide;

            // Decrement slide timer
            slideSecs -= Time.deltaTime;

            if (slideSecs <= 0.0f)
            {
                isSliding = false;
                // Enable jumping and friction again
                rg.velocity = new Vector2(0.0f, 0.0f);
            }
        }
    }

    void AnimateHunterCharacterMovs()
    {
        AnimateSlide();
        AnimateAttackSlash();
    }
}
