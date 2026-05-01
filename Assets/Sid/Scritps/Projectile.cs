using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start()
    {
        // Faz a flecha voar para a direita (frente) assim que nasce
        rb.linearVelocity = transform.right * speed;
        
        // Destrói a flecha após 3 segundos para não pesar o jogo se ela não bater em nada
        Destroy(gameObject, 3f); 
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Ignora o colisor do próprio Damon
        if (hitInfo.CompareTag("Player"))
        {
            return;
        }

        // Exemplo de como dar dano no futuro:
        // if (hitInfo.CompareTag("Enemy")) { hitInfo.GetComponent<Enemy>().TakeDamage(10); }

        // Destrói a flecha ao bater em qualquer outra coisa (chão, parede, inimigo)
        Destroy(gameObject);
    }
}