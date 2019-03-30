using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiefCubeGuard : MonoBehaviour {

    public enum ChiefState
    {
        GoingLaptop,
        Laptop,
        GoingConsole,
        Console,
        GoingWarning,
        Warning,
        GoingGuard,
        Guard,
        Nothing,
        GoingAlarm,
        Alarm,
        Down,
        GoingExitPoint,
        Exit
    }

    public enum StateMovement
    {
        Sitting,
        Idle,
        Walking,
        Running
    }

    public Transform player;
    public AlarmSystem alarmSystem;
    public AlarmTrigger alarmTrigger;
    public TimerManager timerManager;

    public Transform exitSitTransform;
    public Transform enterSitTransform;
    public Transform sitTransform;
    public Transform consoleLookAt;
    public Transform consoleTransform;
    public Transform guardLookAt;
    public Transform guardTransform;
    public Transform alarmLookAt;
    public Transform alarmTransform;
    public Transform exitTransform;

    private Vector3[] path;

    private IEnumerator followPathIEnumerator;
    private ChiefState previousChiefState;
    private ChiefState chiefState;
    private StateMovement stateMovement;
    private ChiefCubeGuardAnimation animator;

    public AudioSource footSound;
    private bool isPlayingFoot;
    private bool alternanceFoot;
    private float footTimer;
    private float footSpeed;

    private bool willTriggerAlarm;
    private bool goingLaptop;
    private bool goingConsole;
    private bool goingGuard;
    private bool goingAlarm;
    private bool goingExit;

    public LayerMask viewMask;
    public float viewDistance = 10.0f;
    public float viewAngle = 65.0f;
    public float speed = 2.0f;
    public float turnSpeed = 10.0f;
    public float timeOnLaptop = 10.0f;
    public float timeOnConsole = 5.0f;
    public float timeGuard = 10.0f;
    public float timeAlarm = 2.0f;
    private float timerOccupation;


    void Start ()
    {
        willTriggerAlarm = false;
        goingLaptop = false;
        goingConsole = false;
        goingGuard = false;
        goingAlarm = false;
        timerOccupation = 0.0f;
        animator = GetComponent<ChiefCubeGuardAnimation>();
        followPathIEnumerator = FollowPath();
        previousChiefState = ChiefState.Nothing;
        chiefState = ChiefState.Nothing;
        stateMovement = StateMovement.Idle;
	}

	void Update ()
    {
        if (chiefState != ChiefState.Down)
        {
            if (animator.walk || animator.run)
            {
                if (!isPlayingFoot)
                {
                    footSound.Play();
                    isPlayingFoot = true;
                }

                if (animator.walk)
                {
                    footSpeed = 0.6f;
                }
                else
                {
                    footSpeed = 0.35f;
                }
                if (footTimer >= footSpeed)
                {
                    alternanceFoot = !alternanceFoot;
                    if (alternanceFoot)
                    {
                        footSound.panStereo = -0.35f;
                        footSound.pitch = Random.Range(0.50f, 1.60f);
                    }
                    else
                    {
                        footSound.panStereo = 0.35f;
                        footSound.pitch = Random.Range(0.50f, 1.60f);
                    }
                    footSound.Stop();
                    footSound.Play();

                    footTimer = 0.0f;
                }
                footTimer += Time.deltaTime;
            }
            else
            {
                isPlayingFoot = false;
                footSound.Stop();
            }


            if (CanSeePlayer() && !willTriggerAlarm)
            {
                goingLaptop = false;
                goingConsole = false;
                goingGuard = false;
                willTriggerAlarm = true;
                animator.Unsit();
                animator.Run();
                speed = 2.0f;
                StopCoroutine(followPathIEnumerator);
                chiefState = ChiefState.GoingAlarm;
            }

            // TRANSITING CHIEF STATE
            if (chiefState == ChiefState.Nothing)
            {
                List<ChiefState> possibilities = PossibleState();
                int randomPosibility = Random.Range(0, possibilities.Count);
                chiefState = possibilities[randomPosibility];
            }
            else if (chiefState == ChiefState.GoingLaptop && !goingLaptop)
            {
                goingLaptop = true;
                PathRequestManager.RequestPath(transform.position, enterSitTransform.position, OnPathFound);
            }
            else if (chiefState == ChiefState.GoingConsole && !goingConsole)
            {
                goingConsole = true;
                PathRequestManager.RequestPath(transform.position, consoleTransform.position, OnPathFound);
            }
            else if (chiefState == ChiefState.GoingGuard && !goingGuard)
            {
                goingGuard = true;
                PathRequestManager.RequestPath(transform.position, guardTransform.position, OnPathFound);
            }
            else if (chiefState == ChiefState.GoingAlarm && !goingAlarm)
            {
                goingAlarm = true;
                PathRequestManager.RequestPath(transform.position, alarmTransform.position, OnPathFound);
            }
            else if(chiefState == ChiefState.GoingExitPoint && !goingExit)
            {
                goingExit = true;
                PathRequestManager.RequestPath(transform.position, exitTransform.position, OnPathFound);
            }

            // GOING TO STATE
            if (goingLaptop)
            {
                float distance = Vector3.Distance(enterSitTransform.position, transform.position);
                if (distance < 0.6f)
                {
                    StopCoroutine(followPathIEnumerator);
                    transform.position = sitTransform.position;
                    transform.rotation = sitTransform.rotation;
                    animator.Sit();
                    chiefState = ChiefState.Laptop;
                    timerOccupation = 0.0f;
                    goingLaptop = false;
                }
            }

            if (goingConsole)
            {
                float distance = Vector3.Distance(consoleTransform.position, transform.position);
                if (distance < 0.65f)
                {
                    StopCoroutine(followPathIEnumerator);
                    Vector3 lookAtPoint = new Vector3(consoleLookAt.transform.position.x, transform.position.y, consoleLookAt.transform.position.z);
                    transform.LookAt(lookAtPoint);
                    timerOccupation = 0.0f;
                    animator.Idle();
                    goingConsole = false;
                    chiefState = ChiefState.Console;
                }
            }

            if (goingGuard)
            {
                float distance = Vector3.Distance(guardTransform.position, transform.position);
                if (distance < 0.8f)
                {
                    StopCoroutine(followPathIEnumerator);
                    Vector3 lookAtPoint = new Vector3(guardLookAt.transform.position.x, transform.position.y, guardLookAt.transform.position.z);
                    transform.LookAt(lookAtPoint);
                    timerOccupation = 0.0f;
                    animator.Idle();
                    goingGuard = false;
                    chiefState = ChiefState.Guard;
                }
            }

            if (goingAlarm)
            {
                float distance = Vector3.Distance(alarmTransform.position, transform.position);
                if (distance < 0.7f)
                {
                    StopCoroutine(followPathIEnumerator);
                    Vector3 lookAtPoint = new Vector3(alarmLookAt.transform.position.x, transform.position.y, alarmLookAt.transform.position.z);
                    transform.LookAt(lookAtPoint);
                    timerOccupation = 0.0f;
                    animator.Idle();
                    goingAlarm = false;
                    chiefState = ChiefState.Alarm;
                }
            }

            if(goingExit)
            {
                float distance = Vector3.Distance(exitTransform.position, transform.position);
                if(distance < 0.7f)
                {
                    StopCoroutine(followPathIEnumerator);
                    gameObject.SetActive(false);
                }
            }

            // MANAGING STATE
            if (chiefState == ChiefState.Laptop)
            {
                timerOccupation += Time.deltaTime;
                if (timerOccupation > Random.Range(timeOnLaptop * 0.8f, timeOnLaptop * 1.2f))
                {
                    previousChiefState = chiefState;
                    chiefState = ChiefState.Nothing;
                    animator.Unsit();
                    animator.Idle();
                    transform.position = exitSitTransform.position;
                    transform.rotation = exitSitTransform.rotation;
                }
            }
            else if (chiefState == ChiefState.Console)
            {
                timerOccupation += Time.deltaTime;
                if (timerOccupation > Random.Range(timeOnConsole * 0.8f, timeOnConsole * 1.2f))
                {
                    previousChiefState = chiefState;
                    chiefState = ChiefState.Nothing;
                }
            }
            else if (chiefState == ChiefState.Guard)
            {
                timerOccupation += Time.deltaTime;
                if (timerOccupation > Random.Range(timeGuard * 0.8f, timeGuard * 1.2f))
                {
                    previousChiefState = chiefState;
                    chiefState = ChiefState.Nothing;
                }
            }
            else if (chiefState == ChiefState.Alarm)
            {
                timerOccupation += Time.deltaTime;
                if (timerOccupation > Random.Range(timeAlarm * 0.8f, timeAlarm * 1.2f))
                {
                    if (alarmTrigger.hasBeenTricked)
                    {
                        chiefState = ChiefState.Down;
                        StartCoroutine(Fall());
                    }
                    else
                    {
                        previousChiefState = chiefState;
                        chiefState = ChiefState.Nothing;
                        willTriggerAlarm = false;
                        alarmSystem.TriggerAlarmSystem();

                        // Timer trigger, play have 30 seconds to get out of the building
                        timerManager.StartTimer();
                        chiefState = ChiefState.GoingExitPoint;
                    }
                }
            }
        }
    }

    public IEnumerator Fall()
    {
        animator.Idle();
        StopCoroutine(followPathIEnumerator);
        chiefState = ChiefState.Down;
        Vector3 axis = Vector3.forward + Vector3.right;
        float angle = -90.0f;
        float fallDuration = 1.0f;
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;

        Vector3 fromPos = transform.position;
        Vector3 toPos = transform.position + new Vector3(0, 0.2f, 0.0f);

        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < fallDuration)
        {
            transform.position = Vector3.Lerp(fromPos, toPos, elapsed / fallDuration);
            transform.rotation = Quaternion.Slerp(from, to, elapsed / fallDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private List<ChiefState> PossibleState()
    {
        List<ChiefState> states = new List<ChiefState>();

        if(previousChiefState != ChiefState.Laptop)
        {
            states.Add(ChiefState.GoingLaptop);
        }

        if(previousChiefState != ChiefState.Console)
        {
            states.Add(ChiefState.GoingConsole);
        }

        if(previousChiefState != ChiefState.Guard)
        {
            states.Add(ChiefState.GoingGuard);
        }

        return states;
    }

    public void OnPathFound(Vector3[] newPath, bool success)
    {
        if (success)
        {
            if(willTriggerAlarm)
            {
                animator.Run();
            }
            else
            {
                animator.Walk();
            }
            
            path = newPath;
            StopCoroutine(followPathIEnumerator);
            followPathIEnumerator = FollowPath();
            StartCoroutine(followPathIEnumerator);
        }
    }

    /*private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }*/

    private IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        Vector3 currentWaypoint = path[0];
        transform.LookAt(currentWaypoint);

        while (followingPath)
        {
            if (transform.position == currentWaypoint)
            {
                pathIndex++;
                if (pathIndex >= path.Length)
                {
                    followingPath = false;
                    break;
                }
                currentWaypoint = path[pathIndex];
            }

            if(followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(path[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = 0; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.25f, 0.25f, 0.25f));

                if (i == 0)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
