using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGuard : MonoBehaviour
{

    public enum StateAnimation
    {
        Idling,
        Walking,
        Running
    }

    public CubeManager cubeManager;
    public FlockingManager flockingManager;
    [HideInInspector]
    public StateAnimation stateAnimation;
    private CubeGuardAnimation animator;
    public float speed = 2.0f;
    public float speedPatrol = 1.0f;
    public float speedChasing = 1.8f;
    public bool down;

    public Transform player;
    public Transform target;
    private Vector3[] path;
    private int targetIndex;
    private bool startChasing;
    private bool isChasing;
    public bool IsChasing
    {
        get { return isChasing; }
        private set { }
    }
    private bool joiningBackPatrolWaypoint;

    [HideInInspector]
    public FlockingManager.FlockingGroup currentFlockingGroup;
    private bool isFlocking;
    public bool IsFlocking
    {
        get { return isFlocking; }
        set { isFlocking = value; }
    }
    private bool isFlockingLeader;
    public bool IsFlockingLeader
    {
        get { return isFlockingLeader; }
        set { isFlockingLeader = value; }
    }
    private CubeGuard flockingLeader;
    public CubeGuard FlockingLeader
    {
        get { return flockingLeader; }
        set { flockingLeader = value; }
    }
    private float timerFlocking;
    public int positionInFlock;


    private int targetWaypointIndex;
    private int saveTargetWaypointIndex;
    private WaypointPatrol[] waypoints;
    private bool isFollowingPatrolPath;
    public Transform pathHolder;
    public float turnSpeed = 90;
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    private float viewAngle;
    private Color originalSpotlight;

    public AudioSource footSound;
    private bool isPlayingFoot;
    private bool alternanceFoot;
    private float footTimer;
    private float footSpeed;

    private IEnumerator turnToFace;
    private IEnumerator followPlayer;
    private IEnumerator followPathCoroutine;

    private CubeGuard[] allGuards;

    private float timePath;
    private float viewPlayerSince;

    public Transform toLook;
    
    void Start()
    {
        isPlayingFoot = false;
        alternanceFoot = false;
        footTimer = 0.0f;
        down = false;
        positionInFlock = 0;
        timerFlocking = 0.0f;
        currentFlockingGroup = null;
        isFlocking = false;
        allGuards = FindObjectsOfType<CubeGuard>();

        joiningBackPatrolWaypoint = false;
        saveTargetWaypointIndex = 0;
        startChasing = false;
        viewPlayerSince = 0.0f;
        timePath = 0.0f;
        isChasing = false;
        isFollowingPatrolPath = false;
        originalSpotlight = spotlight.color;
        viewAngle = spotlight.spotAngle;

        animator = GetComponent<CubeGuardAnimation>();
        animator.Idle();
        stateAnimation = StateAnimation.Idling;

        followPlayer = FollowPath();

        if (pathHolder != null)
        {
            waypoints = new WaypointPatrol[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).GetComponent<WaypointPatrol>();
            }
            
            isFollowingPatrolPath = true;
            followPathCoroutine = FollowPatrolPath(waypoints);
            StartCoroutine(followPathCoroutine);
        }
    }

    public IEnumerator Fall()
    {
        animator.Idle();
        if(turnToFace != null)
        {
            StopCoroutine(turnToFace);
        }
        if(followPlayer != null)
        {
            StopCoroutine(followPlayer);
        }
        if(followPathCoroutine != null)
        {
            StopCoroutine(followPathCoroutine);
        }
        
        Vector3 axis = Vector3.forward + Vector3.right;
        float angle = -90.0f;
        float fallDuration = 1.0f;
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;

        Vector3 fromPos = transform.position;
        Vector3 toPos = transform.position + new Vector3(0, 0.1f, 0.0f);

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
        if(Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if(angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Update()
    {
        if (!down)
        {
            if(animator.walk || animator.run)
            {
                if(!isPlayingFoot)
                {
                    footSound.Play();
                    isPlayingFoot = true;
                }
                
                if(animator.walk)
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

            if (isChasing && Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                cubeManager.Lose(CubeManager.LoseReason.CATCHED);
                return;
            }

            if (isFlocking && !isFlockingLeader)
            {
                Vector3 positionLeaderBehind = -currentFlockingGroup.leader.transform.forward * 2;
                positionLeaderBehind.y = transform.position.y;

                timerFlocking += Time.deltaTime;
                if (timerFlocking > 0.2f)
                {
                    Vector3 direction = Vector3.zero;
                    if (positionInFlock == 0)
                    {
                        direction = currentFlockingGroup.leader.transform.position - (currentFlockingGroup.leader.transform.forward * 0.5f);
                    }
                    else if (positionInFlock == 1)
                    {
                        direction = currentFlockingGroup.leader.transform.position - (currentFlockingGroup.leader.transform.forward * 0.5f) - (currentFlockingGroup.leader.transform.right * 0.5f);
                    }
                    else if (positionInFlock == 2)
                    {
                        direction = currentFlockingGroup.leader.transform.position - (currentFlockingGroup.leader.transform.forward * 0.5f) + (currentFlockingGroup.leader.transform.right * 0.5f);
                    }

                    timerFlocking = 0.0f;
                    PathRequestManager.RequestPath(transform.position, direction, OnPathFound);
                }

                if (!currentFlockingGroup.leader.GetComponent<CubeGuard>().isChasing)
                {
                    isFlocking = false;
                    isChasing = false;
                    startChasing = false;
                    joiningBackPatrolWaypoint = true;
                    speed = speedPatrol;
                    animator.Walk();
                    spotlight.color = originalSpotlight;
                    PathRequestManager.RequestPath(transform.position, waypoints[targetWaypointIndex].transform.position, OnPathFound);
                }
            }
            else
            {
                // Internal guard state
                if (CanSeePlayer())
                {
                    isFollowingPatrolPath = false;
                    isChasing = true;
                    viewPlayerSince = 0.0f;
                }
                else if (isChasing)
                {
                    viewPlayerSince += Time.deltaTime;
                    if (viewPlayerSince >= 1.0f)
                    {
                        if (turnToFace != null)
                        {
                            StopCoroutine(turnToFace);
                        }
                        StopCoroutine(followPlayer);
                        isChasing = false;
                        startChasing = false;
                        spotlight.color = originalSpotlight;
                        speed = speedPatrol;
                        animator.Walk();
                        joiningBackPatrolWaypoint = true;
                        PathRequestManager.RequestPath(transform.position, waypoints[targetWaypointIndex].transform.position, OnPathFound);
                    }
                }

                // Act according to state
                if (isChasing)
                {
                    spotlight.color = Color.red;
                    if (!startChasing)
                    {
                        speed = speedChasing;
                        animator.Run();
                        timePath = 0.0f;
                        startChasing = true;
                        saveTargetWaypointIndex = targetWaypointIndex;
                        if (turnToFace != null)
                        {
                            StopCoroutine(turnToFace);
                        }
                        StopCoroutine(followPathCoroutine);
                        PathRequestManager.RequestPath(transform.position, player.position, OnPathFound);
                    }

                    // Flocking
                    CubeGuard guardNear = NearOtherGuard();
                    if (guardNear != null)
                    {
                        // Join flocking group
                        if (NearGuardIsInFlockingGroup(guardNear))
                        {
                            if (currentFlockingGroup != guardNear.currentFlockingGroup)
                            {
                                // Incorporate existing flocking group
                                StopCoroutine(followPlayer);
                                positionInFlock = guardNear.currentFlockingGroup.AddFlocker(this.gameObject);
                                currentFlockingGroup = guardNear.currentFlockingGroup;
                                isFlocking = true;
                                isFlockingLeader = false;
                            }
                        }
                        // Create new flocking group
                        else
                        {
                            StopCoroutine(followPlayer);
                            StopCoroutine(guardNear.followPlayer);
                            currentFlockingGroup = new FlockingManager.FlockingGroup(this.gameObject, guardNear.gameObject);
                            guardNear.currentFlockingGroup = currentFlockingGroup;
                            guardNear.positionInFlock = 0;
                            flockingManager.AddFlockingGroup(currentFlockingGroup);
                            isFlocking = true;
                            isFlockingLeader = true;
                            guardNear.isFlocking = true;
                        }
                    }

                    // Rechercher le chemin
                    timePath += Time.deltaTime;
                    if (timePath >= 0.5f)
                    {
                        timePath = 0.0f;
                        PathRequestManager.RequestPath(transform.position, player.position, OnPathFound);
                    }
                }

                if (joiningBackPatrolWaypoint)
                {
                    float distance = Vector3.Distance(transform.position, waypoints[targetWaypointIndex].transform.position);
                    if (distance <= 0.6f)
                    {
                        StopCoroutine(followPlayer);
                        animator.Idle();
                        joiningBackPatrolWaypoint = false;
                        followPathCoroutine = FollowPatrolPath(waypoints);
                        StartCoroutine(followPathCoroutine);
                    }
                }
            }
        }
    }

    private List<CubeGuard> GuardsInFlocking()
    {
        List<CubeGuard> guardsIn = new List<CubeGuard>();
        foreach(CubeGuard guard in allGuards)
        {
            if(guard.isChasing && Vector3.Distance(transform.position, guard.transform.position) < 3.0)
            {
                guardsIn.Add(guard);
            }
        }

        return guardsIn;
    }

    private CubeGuard NearOtherGuard()
    {
        foreach (CubeGuard guard in allGuards)
        {

            if (guard != this && guard.isChasing && Vector3.Distance(transform.position, guard.transform.position) < 1.0)
            {
                return guard;
            }
        }

        return null;
    }

    private bool NearGuardIsInFlockingGroup(CubeGuard nearGuard)
    {
        return nearGuard.isChasing && nearGuard.currentFlockingGroup != null;
    }

    private IEnumerator FollowPatrolPath(WaypointPatrol[] waypoints)
    {
        //transform.position = waypoints[0].transform.position;

        targetWaypointIndex = saveTargetWaypointIndex;
        WaypointPatrol targetWaypoint = waypoints[targetWaypointIndex];

        /*Vector3 lookAtPoint = new Vector3(targetWaypoint.transform.position.x, transform.position.y, targetWaypoint.transform.position.z);
        transform.LookAt(lookAtPoint);*/
        

        while(true)
        {
            stateAnimation = StateAnimation.Walking;
            
            animator.Walk();
            Vector3 lookAtPoint = new Vector3(waypoints[targetWaypointIndex].transform.position.x, transform.position.y, waypoints[targetWaypointIndex].transform.position.z);
            transform.LookAt(lookAtPoint);
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.transform.position, speed * Time.deltaTime);

            if(transform.position == targetWaypoint.transform.position)
            {
                if (waypoints[targetWaypointIndex].waitTime > -1)
                {
                    float toWait = waypoints[targetWaypointIndex].waitTime;
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = waypoints[targetWaypointIndex];
                    animator.Idle();
                    stateAnimation = StateAnimation.Idling;
                    turnToFace = TurnToFaceWaypoint(targetWaypoint.transform);
                    yield return StartCoroutine(turnToFace);
                    yield return new WaitForSeconds(toWait);
                }
                else
                {
                    animator.Idle();
                    Vector3 lookAt2 = new Vector3(toLook.position.x, transform.position.y, toLook.position.z);
                    transform.LookAt(lookAt2);
                }

            }
            yield return null;
        }
    }
    private IEnumerator TurnToFaceWaypoint(Transform waypointToFace)
    {
        Vector3 dirToLook = (waypointToFace.position - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLook.z, dirToLook.x) * Mathf.Rad2Deg;

        while(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool success)
    {
        if(success)
        {
            StopCoroutine(followPlayer);
            path = newPath;
            followPlayer = FollowPath();
            StartCoroutine(followPlayer);
        }
    }

    /*private IEnumerator FollowPath()
    {
        if(path.Length > 0)
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
        }
    }*/

    private IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        if(path.Length > 0)
        {
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
                        yield break;
                    }
                    currentWaypoint = path[pathIndex];
                }

                if (followingPath)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(path[pathIndex] - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if(pathHolder != null)
        {
            Vector3 startPosition = pathHolder.GetChild(0).position;
            Vector3 previousPosition = startPosition;
            foreach (Transform waypoint in pathHolder)
            {
                Gizmos.DrawSphere(waypoint.position, 0.3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.25f, 0.25f, 0.25f));

                if(i == targetIndex)
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
