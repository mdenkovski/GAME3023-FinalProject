using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField]
    Animator animator;

    [SerializeField]
    Direction walkDirection = Direction.SOUTH;

    [SerializeField]
    Rigidbody2D rigidbody;

    

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isWalking = vel.magnitude > float.Epsilon;
        if (isWalking)
        {
            if (math.abs(vel.x) > math.abs(vel.y)) //moving left/right more than up/down
            {
                if (vel.x < 0) //moving left/WEST
                {
                    walkDirection = Direction.WEST;
                }
                else //moving right/EAST
                {
                    walkDirection = Direction.EAST;
                }
            }
            else //moving more vertically
            {
                if (vel.y < 0) //moving down/SOUTH
                {
                    walkDirection = Direction.SOUTH;
                }
                else //moving up/NORTH
                {
                    walkDirection = Direction.NORTH;
                }
            }
        }

        //only change direction when walking
        animator.SetInteger("Direction", (int)walkDirection);
        //tall animator is we are walking or not
        animator.SetBool("IsWalking", isWalking);
    }
}
