using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float crouchTimer = 1;

    bool crouching = false;
    bool lerpCrouch = false;
    bool sprinting = false;

    private Vector3 lastMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
/*            
            else if (crouching && !isGrounded)
            {
                controller.height = Math.Lerp(controller.height, 2, p);
            }
*/
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }
            
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }


    }

    //receive inputs from InputManager.cs script and apply them to character controller
    public void ProcessMove(Vector2 input)
    {
        /*        
                Vector3 moveDirection = Vector3.zero;
                moveDirection.x = input.x;
                moveDirection.z = input.y;
                controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

                playerVelocity.y += gravity * Time.deltaTime;
                if (isGrounded && playerVelocity.y < 0)
                {
                    playerVelocity.y = -2f;
                }

                controller.Move(playerVelocity * Time.deltaTime);
                Debug.Log(playerVelocity.y); 
        */

        //Gravity
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        //Grounded
        if (isGrounded)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;

            lastMoveDirection = moveDirection;

            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        }

        //OnAir
        else
        {
            controller.Move(transform.TransformDirection(lastMoveDirection) * speed * Time.deltaTime);
        }

        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }

    public void jump()
    {
        if (isGrounded && !crouching)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }
    }
}