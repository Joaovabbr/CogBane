using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int projDamage = 50;
    public float lifeTime = 3f; 
    public GameObject explosionEffectPrefab; 
    
    public AudioClip somExplosao;

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
        else if (hit.gameObject.layer == LayerMask.NameToLayer("chao_cenario")) 
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
        if (somExplosao != null)
        {
            AudioSource.PlayClipAtPoint(somExplosao, transform.position);
        }
        Destroy(gameObject);
    }
}