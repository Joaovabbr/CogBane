using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
        
        Destroy(gameObject, 3f); 
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            return;
        }

        // Exemplo de como dar dano no futuro:
        // if (hitInfo.CompareTag("Enemy")) { hitInfo.GetComponent<Enemy>().TakeDamage(10); }

        Destroy(gameObject);
    }
}