using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private Vector3 direction;
    //sets direction of the bullet and to selfdestroy after a time
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //add later
        }
    }
}

