using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float danoAtaque = 7.5f;
    private Rigidbody2D rb;

    [Header("Áudio de Impacto")]
    public AudioClip somImpacto; // Arraste o impacto_besta.wav aqui

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 0.45f); 
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.isTrigger || hit.CompareTag("Player")) return;

        if (hit.CompareTag("Enemy"))
        {
            if (hit.TryGetComponent(out Entity scriptInimigo))
            {
                scriptInimigo.TomarDano(danoAtaque, "enemy");

                // Toca o som no local do impacto antes da flecha sumir
                if (somImpacto != null)
                {
                    AudioSource.PlayClipAtPoint(somImpacto, Camera.main.transform.position, 1f);
                }
            }
        }
        
        Destroy(gameObject);
    }
}