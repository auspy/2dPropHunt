using System;
using System.Collections;
using UnityEngine;

public class Hunter : Character
{
    // VARIABLES
    // movements
    bool isAttacking = false;

    // char animation states
    public int stateAttackSlash = 6;
    public int stateAttckKick = 7;

    // FUNCTIONS
    public Hunter()
    {
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveCharacter();
        CharacterAnimations();
moveHunter();
        AnimateAttackSlash();
        AnimateSlide();
    }

    public void moveHunter()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            print("attacking");
            isAttacking = true;
        }
        else if (Input.GetKey(KeyCode.X) && isMoving && !isSliding)
        {
            print("sliding");
            isSliding = true;
            rg.velocity =new Vector2(movX * (runSpeed), movY) ;
        }
        else
        {
            isAttacking = false;
            isSliding = false;
        }
        
    }

    void AnimateAttackSlash()
    {
        if (isAttacking)
        {
            // attack animate
            SetAnimation(stateAttackSlash);
            //yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            //yield return null;
            //print("can attack in "+animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    void AnimateSlide()
    {
        if (isSliding)
        {
            SetAnimation(stateSlide);
        }
    }

}
