using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Áudio")]
    public AudioSource audioSource; 
    public AudioClip somDisparoBesta;
    public AudioClip somEstocada;

    [Header("Estado de Combate")]
    public bool isAttacking = false;

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

    [Header("Balanceamento (Cooldown)")]
    public float tempoRecargaBesta = 0.75f;
    private float cronometroRecarga = 0f;

    private Animator anim;
    private PlayerEntity atributos;

    void Start()
    {
        anim = GetComponent<Animator>();
        atributos = GetComponent<PlayerEntity>();
        
        if(audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (atributos.vidaAtual <= 0) return;

        if (cronometroRecarga > 0)
        {
            cronometroRecarga -= Time.deltaTime;
        }

        if (isAttacking) return;

        bool apertouAtaqueCurto = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J);
        bool apertouAtaqueLongo = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K);

        if (apertouAtaqueCurto) 
        { 
            anim.SetTrigger("attackShort");
            if (audioSource != null && somEstocada != null)
            {
                audioSource.PlayOneShot(somEstocada);
            }
            TravarCombate(); 
        }

        if (apertouAtaqueLongo && cronometroRecarga <= 0f) 
        { 
            anim.SetTrigger("attackLong"); 
            TravarCombate(); 
            cronometroRecarga = tempoRecargaBesta; 
        }
    }

    public void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            if (audioSource != null && somDisparoBesta != null)
            {
                audioSource.PlayOneShot(somDisparoBesta);
            }
        }
    }

    public void TravarCombate() => isAttacking = true;
    public void DestravarCombate() => isAttacking = false;

    public void DispararEfeitoEstocada() { if (vfxAnimator != null) vfxAnimator.SetTrigger("Attack"); }

    public void CastAttackHitbox()
    {   
        float hitOffset = isFlipped ? -offset_x : offset_x;
        
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(hitOffset, offset_y);
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0f);
        
        foreach (Collider2D hit in hits)
        {
            // SEGURANÇA ANTIGATILHO: 
            // Se o colisor for um Trigger (área de visão/patrulha), ignora e vai para o próximo
            if (hit.isTrigger) continue;

            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out Entity scriptInimigo))
            {
                scriptInimigo.TomarDano(danoAtaque, "enemy");
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