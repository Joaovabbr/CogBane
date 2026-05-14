using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float danoAtaque = 7.5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 0.45f); 
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.isTrigger) return;
        if (hit.CompareTag("Player")) return;

        if (hit.CompareTag("Enemy"))
        {
            if (hit.TryGetComponent(out Entity scriptInimigo))
            {

                scriptInimigo.TomarDano(danoAtaque, "enemy");
            }
        }
        Destroy(gameObject);
    }
}