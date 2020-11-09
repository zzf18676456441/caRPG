using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAnimator : MonoBehaviour
{
    public Animator animator;
    public void Idle()
    {
        animator.Play("ShooterIdle");
        print("idle");
    }
    public void Walk()
    {
        animator.Play("ShooterWalk");
        print("walking");
    }
    public void Shoot()
    {
        animator.SetTrigger("fire");
        animator.Play("ShooterFire");
        animator.ResetTrigger("fire");
        print("firing");
    }
}
