using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 3f;
    public float detectionRange = 10f;
    public float shootingInterval = 1.5f;
    public float returnToPatrolTime = 3f;
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private int currentPatrolIndex = 0;
    private bool isChasing = false;
    private float lastShotTime;
    private float lastSeenTime;

    void Update()
    {

        //looks to see if the enemy can see the player, if so, then chase and shoot, otherwise go back to its patrol route after an amount of time
        if (CanSeePlayer())
        {
            isChasing = true;
            lastSeenTime = Time.time;
            ShootAtPlayer();
        }
        else if (isChasing && Time.time - lastSeenTime > returnToPatrolTime)
        {
            isChasing = false;
        }

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
        if (player == null) return false;

        Vector2 directionToPlayer = player.position - transform.position;
        if (directionToPlayer.magnitude > detectionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
        return hit.collider != null && hit.collider.transform == player;
    }

    void ShootAtPlayer()
    {
        if (Time.time - lastShotTime > shootingInterval)
        {
            lastShotTime = Time.time;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
}

