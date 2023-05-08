using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Character : NetworkBehaviour
{
    // In the NetworkManager script:
    public GameObject characterPrefab;

    [HideInInspector]
    public float speed;
    public float runSpeed,
        walkSpeed;
    public float jumpForce = 5f;
    public float maxSlideSecs = 0.5f;

    [HideInInspector]
    public float slideSecs;

    [HideInInspector]
    public float slideX,
        slideY;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public Rigidbody2D rg;

    [HideInInspector]
    public float movX,
        movY;

    // char animation states
    public const int stateIdle = 0;
    public const int stateIdleBlinking = 1;
    public const int stateWalk = 2;
    public const int stateRun = 3;
    public const int stateSlide = 4;
    public const int stateJump = 5;

    public int currentAnimateState = 0;

    [HideInInspector]
    public bool isMoving,
        isJumping,
        isRunning,
        isSliding;

    private GameObject localGameObject;

    // ON START OF SERVER SET INITIAL PLAYER DATA
    public override void OnStartServer()
    {
        speed = walkSpeed;
        movX = 0f;
        movY = 0f;
        isMoving = false;
        isJumping = false;
        isRunning = false;
    }

    //method to set the initial state of the Character on the client-side.
    public override void OnStartClient()
    {
        localGameObject = NetworkClient.localPlayer?.gameObject;
        print("client started " + localGameObject?.name);
        animator = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        cmdMoveCharacter();
        CmdCharacterAnimations(currentAnimateState);
    }

    // [Command]
    public void cmdMoveCharacter()
    {
        if (!isLocalPlayer)
            return;
        speed = walkSpeed;
        if (Input.GetKeyDown(KeyCode.C) && !isJumping)
        {
            isJumping = true;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
            isJumping = false;
        }

        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");
        rg.velocity = new Vector3(movX, movY, 0f) * speed;
        RpcUpdateMovementServer(rg.position, rg.velocity);
    }

    public void AnimateMov()
    {
        Vector3 dir = new(0.2f, 0.2f, 0.2f);
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
            currentAnimateState = stateWalk;
            transform.localScale = dir;
            RpcUpdateDirectionServer(dir);
        }
    }

    public void AnimateIdle()
    {
        if (!(isMoving && isSliding && isJumping))
        {
            currentAnimateState = stateIdle;
        }
    }

    public void AnimateJump()
    {
        if (isJumping)
        {
            print("jumped");
            rg.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            currentAnimateState = stateJump;
        }
    }

    public void AnimateRun()
    {
        if (isRunning && isMoving)
        {
            print("running");
            speed = runSpeed;
            currentAnimateState = stateRun;
        }
    }

    // [Command]
    public void CmdCharacterAnimations(int state)
    {
        SetAnimationServer(state);
        // CmdSetAnimation(state);
    }

    public void AnimateBasicCharacterMovs()
    {
        AnimateIdle();
        AnimateJump();
        AnimateMov();
        AnimateRun();
    }

    [ClientRpc]
    void SetAnimation(int state)
    {
        if (state != 0)
            print(state + " currentAnimateState");
        animator.SetInteger("charState", state);
    }
    [Command]
    void SetAnimationServer(int state)
    {
        animator.SetInteger("charState", state);
        SetAnimation(state);
    }

    [ClientRpc]
    void RpcUpdateMovement(Vector3 position, Vector3 velocity)
    {
        // Update the position and velocity of the character on all clients
        transform.position = position;
        rg.velocity = velocity;
    }
    [Command]
    void RpcUpdateMovementServer(Vector3 position, Vector3 velocity)
    {
        // Update the position and velocity of the character on all clients
        transform.position = position;
        rg.velocity = velocity;
         RpcUpdateMovement(position,velocity);
    }

    // TO SEND PLAYER DIRECTION TO SERVER THEN TO ALL CLIENTS
    [ClientRpc]
    public void RpcUpdateDirection(Vector3 dir)
    {
        transform.localScale = dir;
    }

    [Command]
    public void RpcUpdateDirectionServer(Vector3 dir)
    {
        transform.localScale = dir;
        RpcUpdateDirection(dir);
    }
}
