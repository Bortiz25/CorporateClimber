using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void SetAnimationDirection(Vector2 dir){
        playerAnimator.SetFloat("DirectionX", dir.x);
        playerAnimator.SetFloat("DirectionY", dir.y);
    }

    public void SetAnimationWalkBool(bool isWalking){
        playerAnimator.SetBool("isWalking",isWalking);
    }

    public void SetAnimationRollBool(bool isRolling){
        playerAnimator.SetBool("isRolling", isRolling);
    }
}