using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private void Update()
    {
        float inputX = GameManager.Instance.Joystick.Horizontal;
        float inputY = GameManager.Instance.Joystick.Vertical;

        UpdateMotor(new Vector3(inputX, inputY, 0).normalized);

        Animator animator = GameManager.Instance.playerAnimator;

        animator.SetFloat("moveX", inputX);
        animator.SetFloat("moveY", inputY);

        if (inputX != 0)
        {
            animator.SetFloat("prevMoveX", inputX);
        }
    }
}
