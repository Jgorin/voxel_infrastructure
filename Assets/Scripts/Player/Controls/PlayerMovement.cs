using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10.0f;

    Vector3 velocity;

    void Update(){
        bool isGrounded = controller.isGrounded;
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x * speed + transform.forward * z * speed;
        controller.Move(move * Time.deltaTime);
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // if space down, jump (only if grounded)
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            velocity.y = Mathf.Sqrt(2f * -2f * Physics.gravity.y);
        }

        // if shift down, sprint
        if(Input.GetKey(KeyCode.LeftShift)){
            speed = 20.0f;
        } else {
            speed = 10.0f;
        }
    }
}
