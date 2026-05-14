using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Armas e Efeitos")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Animator vfxAnimator;
    
    [Header("Configurações de Ataque Físico")]
    public float danoAtaque = 15f; 
    public Vector2 hitboxSize = new Vector2(2f, 2.5f); 
    public bool isFlipped = false;
    public float offset_x;
    public float offset_y;

    private Animator anim;
    private PlayerEntity atributos;
    
    

    void Start()
    {
        anim = GetComponent<Animator>();
        atributos = GetComponent<PlayerEntity>();
    }

    void Update()
    {
        if (atributos.vidaAtual <= 0) return;

        bool apertouAtaqueCurto = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J);
        bool apertouAtaqueLongo = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K);

        if (apertouAtaqueCurto) anim.SetTrigger("attackShort");
        if (apertouAtaqueLongo) anim.SetTrigger("attackLong");
        
        
    }

    public void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void DispararEfeitoEstocada()
    {
        if (vfxAnimator != null)
        {
            vfxAnimator.SetTrigger("Attack"); 
        }
    }
    public void CastAttackHitbox()
    {   
        offset_x = isFlipped ? -offset_x : offset_x;
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(offset_x, offset_y);
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy")) 
            {
                if (hit.TryGetComponent(out Entity scriptInimigo))
                {
                    Vector2 direcaoDoGolpe = hit.transform.position - transform.position;
                    scriptInimigo.TomarDano(danoAtaque, "enemy");
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        float gizmoOffset = isFlipped ? -offset_x : offset_x;
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(gizmoOffset, offset_y);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(hitboxPos, hitboxSize); 
    }
}
