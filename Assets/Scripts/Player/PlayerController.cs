using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    private Interactable targetInteractable;

    public bool isHarvesting;
    private PlayerMotor motor;

    public LayerMask mask;
    public Camera mainCamera;
    public GUIManager guiManager;

    private void Start()
    {
        isHarvesting = false;
        targetInteractable = null;
        motor = GetComponent<PlayerMotor>(); 
    }
    
    private void Update()
    {
        if (!guiManager.InMenu() && !isHarvesting)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, int.MaxValue, mask))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("World"))
                    {
                        motor.Stop();
                        targetInteractable = null;
                        motor.MoveTo(hit.point);
                    }
                    else
                    {
                        Interactable interactable = hit.transform.GetComponent<Interactable>();
                        if (interactable != null && interactable.enabled)
                        {
                            targetInteractable = interactable;
                            motor.MoveToInteractable(interactable);
                        }
                    }
                }
            }

            if (targetInteractable != null)
            {
                float distance = Vector3.Distance(transform.position, targetInteractable.transform.position);
                if (distance <= targetInteractable.radius)
                {
                    motor.Stop();
                    transform.LookAt(targetInteractable.transform.position);
                    targetInteractable.Interact(GetComponent<Player>());
                    targetInteractable = null;
                }
                else
                {
                    //motor.MoveToInteractable(targetInteractable);
                }
            }
        }


    }
}
