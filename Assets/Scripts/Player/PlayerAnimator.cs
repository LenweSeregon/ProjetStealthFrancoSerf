using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    const float LOCOMOTION_ANIMATION_SMOOTH_TIME = 0.1f;

    public AudioManager audioManager;
    public Transform attachPointAxe;
    public Transform attachPointPickaxe;

    [HideInInspector]
    public GameObject toolToInstantiate;
    private GameObject toolInstantiated;

    private Animator animator;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("SpeedPercent", speedPercent, LOCOMOTION_ANIMATION_SMOOTH_TIME, Time.deltaTime);
	}

    public void SetIsMining(bool value)
    {
        if (value)
        {
            audioManager.PlayPickaxe();
            toolInstantiated = Instantiate(toolToInstantiate);
            toolInstantiated.transform.SetParent(attachPointPickaxe, true);
            toolInstantiated.transform.position = attachPointPickaxe.position;
            toolInstantiated.transform.rotation = attachPointPickaxe.rotation;
            toolInstantiated.transform.localScale = attachPointPickaxe.localScale;
        }
        else
        {
            audioManager.StopPickaxe();
            Destroy(toolInstantiated);
            toolInstantiated = null;
        }

        animator.SetBool("IsMining", value);
    }

    public void SetIsChopping(bool value)
    {
        if(value)
        {
            audioManager.PlayAxe();
            toolInstantiated = Instantiate(toolToInstantiate);
            toolInstantiated.transform.SetParent(attachPointAxe, true);
            toolInstantiated.transform.position = attachPointAxe.position;
            toolInstantiated.transform.rotation = attachPointAxe.rotation;
            toolInstantiated.transform.localScale = attachPointAxe.localScale;
        }
        else
        {
            audioManager.StopAxe();
            Destroy(toolInstantiated);
            toolInstantiated = null;
        }

        animator.SetBool("IsChopping", value);
    }
}
