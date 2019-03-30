using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGuardAnimation : MonoBehaviour {

    const float LOCOMOTION_ANIMATION_SMOOTH_TIME = 0.1f;
    private Animator animator;
    private CubeGuard guard;
    private bool asked = false;

    [HideInInspector]
    public bool idle;
    [HideInInspector]
    public bool walk;
    [HideInInspector]
    public bool run;
    
	void Awake ()
    {
        idle = false;
        walk = false;
        run = false;
        guard = GetComponent<CubeGuard>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Idle()
    {
        idle = true;
        walk = false;
        run = false;
        float speedPercent = 0.0f;
        animator.SetFloat("SpeedPercent", speedPercent, 0, Time.deltaTime);
    }

    public void Walk()
    {
        idle = false;
        walk = true;
        run = false;
        float speedPercent = 0.5f;
        animator.SetFloat("SpeedPercent", speedPercent, 0, Time.deltaTime);
    }

    public void Run()
    {
        idle = false;
        walk = false;
        run = true;
        float speedPercent = 1.0f;
        animator.SetFloat("SpeedPercent", speedPercent, 0, Time.deltaTime);
    }
   
}
