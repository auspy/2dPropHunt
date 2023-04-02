using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed;

    private Animator animator;
    private float movX,movY;
    [HideInInspector]
    public int charState;

    // char animation states
    public int stateIdle = 0;
    public int stateIdleBlinking = 1;
    public int stateWalk = 2;
    public int stateRun = 3;
    public int stateSlide = 4;
    public int stateJump = 5;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveCharacter();
    }

    void FixedUpdate()
    {
        animate();
    }

    void moveCharacter()
    {
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");
        GetComponent<Rigidbody2D>().velocity= new Vector3(movX, movY, 0f) * speed;
        //print("movX "+movX);
        //print("movY " + movY);
    }

    void animate()
    {
        Vector3 dir  = new(0.2f, 0.2f, 0.2f);
        
        if (movX >0)
        {
            // moving right
            dir.x = 0.2f;

        }else if (movX < 0)
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
            transform.localScale = dir;
            animator.SetInteger("charState", stateWalk);
            //print("moving");
        }
        else 
        {
            // not moving
            animator.SetInteger("charState", stateIdle);
            //print("idle
        }
    }
}
