using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    bool jump = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) 
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        controller.Move(0f, false, jump);
        jump = false;
    }
}
