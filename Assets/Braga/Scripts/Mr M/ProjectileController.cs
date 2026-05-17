using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int projDamage = 40;
    public float lifeTime = 3f; 
    public GameObject explosionEffectPrefab; 

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            if (hit.TryGetComponent(out Entity scriptInimigo))
            {
                scriptInimigo.TomarDano(projDamage, "boss_projectile");
            }
            ExplodeAndDestroy();
        }
        else if (hit.CompareTag("Ground")) 
        {
            ExplodeAndDestroy();
        }
    }

    private void ExplodeAndDestroy()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}