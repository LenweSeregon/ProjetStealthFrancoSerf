using UnityEngine;
using TMPro; 

[RequireComponent(typeof(PlayerMotorCube))]
public class PlayerControllerCube : MonoBehaviour
{
    [HideInInspector]
    public bool isEnigma;
    [HideInInspector]
    public bool isInventory;

    private float speed = 0.0f;
    public float Speed
    {
        get { return speed; }
        private set { }
    }

    [SerializeField]
    private float speedWalk = 1.5f;
    public float SpeedWalk
    {
        get { return speedWalk; }
        private set { }
    }
    [SerializeField]
    private float speedRun = 2.5f;
    public float SpeedRun
    {
        get { return speedRun; }
        private set { }
    }

    [SerializeField]
    private float lookSpeed = 3f;
    [SerializeField]
    private int interactionDistance = 3;
    private Interactable possibleInteractable;

    [SerializeField]
    private TextMeshProUGUI interactText;
    [SerializeField]
    private Camera mainCamera;
    private PlayerMotorCube motor;
    private PlayerAnimatorCube animator;
    
	void Start ()
    {
        isInventory = false;
        isEnigma = false;
        possibleInteractable = null;
        motor = GetComponent<PlayerMotorCube>();
        animator = GetComponent<PlayerAnimatorCube>();
	}
	
	void Update ()
    {
        if(!isEnigma && !isInventory)
        {
            ManageMovement();
            ManageRotationY();
            ManageRotationX();

            CheckInteractable();
            TryInteract();
        }
	}

    void CheckInteractable()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactionDistance))
        {
            if(hit.transform.gameObject.GetComponent<Interactable>() != null && hit.transform.gameObject.GetComponent<Interactable>().IsInteractable())
            {
                possibleInteractable = hit.transform.gameObject.GetComponent<Interactable>();
                interactText.gameObject.SetActive(true);
                interactText.text = I18nManager.Fields[possibleInteractable.GetInteractableTextI18nID()];
            }
            else
            {
                possibleInteractable = null;
                interactText.gameObject.SetActive(false);
            }
        }
        else
        {
            possibleInteractable = null;
            interactText.gameObject.SetActive(false);
        }
    }

    void TryInteract()
    {
        if(possibleInteractable != null && Input.GetKeyDown(KeyCode.F))
        {
            possibleInteractable.Interact(GetComponent<Player>());
        }
    }

    void ManageMovement()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 movementHorizontal = transform.right * xMove;
        Vector3 movementVertical = transform.forward * zMove;
        Vector3 movementVector = (movementHorizontal + movementVertical).normalized;

        if(movementVector.magnitude == 0)
        {
            speed = 0.0f;
            animator.Idle();
        }
        else
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = speedRun;
                animator.Run();
            }
            else
            {
                speed = speedWalk;
                animator.Walk();
            }
        }

        Vector3 velocity = movementVector * speed;
        motor.Move(velocity);
    }

    void ManageRotationY()
    {
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSpeed;
        motor.Rotate(rotation);
    }

    void ManageRotationX()
    {
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * lookSpeed;
        motor.RotateCamera(cameraRotationX);
    }
}
