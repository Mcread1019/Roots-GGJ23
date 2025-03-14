using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;  // Set these in the inspector
    public float patrolSpeed = 5f;
    private int currentPatrolIndex = 0;

    [Header("Chase Settings")]
    public float chaseSpeed = 7f;
    public float sightDistance = 15f;
    public float fieldOfViewAngle = 45f;
    public float chaseDurationAfterLost = 2f;
    private float chaseTimer;

    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        // Assumes the player has the tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
        chaseTimer = chaseDurationAfterLost;
    }

    void Update()
    {
        // Check if the enemy can see the player
        if (CanSeePlayer())
        {
            isChasing = true;
            chaseTimer = chaseDurationAfterLost; // reset the timer when the player is seen
        }
        else if (isChasing)
        {
            // Countdown until the enemy stops chasing
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0f)
                isChasing = false;
        }

        // Decide behavior based on state
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > sightDistance)
            return false;

        // Check if within field of view
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfViewAngle)
            return false;

        // Optionally: Use Raycast to check for obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, sightDistance))
        {
            if (hit.transform != player)
                return false;
        }

        return true;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        // Check if reached the current patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    // Optional: Visualize the enemy's field of view in the scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle, 0) * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * sightDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * sightDistance);
    }
}

