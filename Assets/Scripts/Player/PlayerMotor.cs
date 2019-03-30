using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    private NavMeshAgent agent;
    private float sinceLast;
    private bool alternance;
    public float footSpeed;
    public AudioManager audioManager;

    private void Start()
    {
        alternance = false;
        sinceLast = 0.0f;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(agent.velocity.magnitude >= 0.05f)
        {
            if (sinceLast >= footSpeed)
            {
                alternance = !alternance;
                if (alternance)
                {
                    audioManager.footSand.panStereo = -0.35f;
                    audioManager.footSand.pitch = Random.Range(0.50f, 1.60f);
                }
                else
                {
                    audioManager.footSand.panStereo = 0.35f;
                    audioManager.footSand.pitch = Random.Range(0.50f, 1.60f);
                }
                
                audioManager.PlayFootSand();
                this.sinceLast = 0.0f;
            }
        }

        sinceLast += Time.deltaTime;
    }

    public void MoveTo(Vector3 destination)
    {
        agent.stoppingDistance = 0.0f;
        agent.SetDestination(destination);
    }

    public void MoveToInteractable(Interactable interactable)
    {
        agent.stoppingDistance = interactable.radius * 0.5f;
        agent.SetDestination(interactable.transform.position);
    }

    public void Stop()
    {
        agent.ResetPath();
    }
}
