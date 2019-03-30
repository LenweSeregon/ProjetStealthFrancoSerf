using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorCube : MonoBehaviour
{
    const float LOCOMOTION_ANIMATION_SMOOTH_TIME = 0.1f;
    
    private Animator animator;
    private PlayerMotorCube motor;
    private PlayerControllerCube controller;
    
    void Start ()
    {
        motor = GetComponent<PlayerMotorCube>();
        controller = GetComponent<PlayerControllerCube>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Idle()
    {
        float speedPercent = 0.0f;
        animator.SetFloat("SpeedPercent", speedPercent, 0, Time.deltaTime);
    }

    public void Walk()
    {
        float speedPercent = 0.5f;
        animator.SetFloat("SpeedPercent", speedPercent, 0, Time.deltaTime);
    }

    public void Run()
    {
        float speedPercent = 1.0f;
        animator.SetFloat("SpeedPercent", speedPercent, 0, Time.deltaTime);
    }
}
