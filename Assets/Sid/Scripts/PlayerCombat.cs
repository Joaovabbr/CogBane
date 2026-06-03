using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Referências")]
    public StatusJogadorSO statusDamon; 

    [Header("Áudio")]
    public AudioSource audioSource; 
    public AudioClip somDisparoBesta;
    public AudioClip somEstocada;
    public AudioClip[] sonsGarra; 

    [Header("Estado de Combate")]
    public bool isAttacking = false;

    [Header("Armas e Efeitos")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Animator vfxAnimator;
    
    [Header("Configurações de Ataque Físico")]
    public float danoAtaque = 15f; 
    public float danoGarra = 20f;
    public Vector2 hitboxSize = new Vector2(2f, 2.5f); 
    public bool isFlipped = false;
    public float offset_x;
    public float offset_y;

    [HideInInspector] public bool mobileDagger = false;
    [HideInInspector] public bool mobileBesta = false;
    [HideInInspector] public bool mobileGarra = false;

    [Header("Balanceamento (Cooldown)")]
    public float tempoRecargaBesta = 0.75f;
    private float cronometroRecarga = 0f;
    
    public float tempoRecargaGarra = 1.5f;
    private float cronometroRecargaGarra = 0f;

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
        
        isFlipped = (transform.eulerAngles.y != 0) || (transform.localScale.x < 0);

        if (cronometroRecarga > 0)
        {
            cronometroRecarga -= Time.deltaTime;
        }
        
        if (cronometroRecargaGarra > 0)
        {
            cronometroRecargaGarra -= Time.deltaTime;
        }

        if (isAttacking) return;

        bool apertouAtaqueCurto = Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J) || mobileDagger;
        bool apertouAtaqueLongo = Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K) || mobileBesta;
        bool apertouAtaqueGarra = Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.L) || mobileGarra;
        mobileDagger = false;
        mobileBesta = false;
        mobileGarra = false;

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
        
        if (apertouAtaqueGarra && statusDamon.garraDesbloqueada && cronometroRecargaGarra <= 0f)
        {
            anim.SetTrigger("clawAttack");
            
            if (audioSource != null && sonsGarra.Length > 0)
            {
                int randomIndex = Random.Range(0, sonsGarra.Length);
                AudioClip somSorteado = sonsGarra[randomIndex];
                if (somSorteado != null)
                {
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(somSorteado);
                }
            }

            TravarCombate();
            cronometroRecargaGarra = tempoRecargaGarra;
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

    public void DispararEfeitoEstocada() 
    { 
        if (vfxAnimator != null) 
        {
            vfxAnimator.SetTrigger("Attack");
            FlipVFX();
        }
    }
    
    public void DispararEfeitoGarra() 
    { 
        if (vfxAnimator != null) 
        {
            vfxAnimator.SetTrigger("Claw");
            FlipVFX();
        }
    }

    private void FlipVFX()
    {
        if (vfxAnimator != null)
        {
            SpriteRenderer vfxSprite = vfxAnimator.GetComponent<SpriteRenderer>();
            if (vfxSprite != null)
            {
                vfxSprite.flipX = false;
            }
        }
    }
    
    public void CastAttackHitbox()
    {   
        float hitOffset = isFlipped ? -offset_x : offset_x;
        
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(hitOffset, offset_y);
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0f);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.isTrigger) continue;

            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out Entity scriptInimigo))
            {
                scriptInimigo.TomarDano(danoAtaque, "enemy");
            }
        }
    }
    
    public void CastClawHitbox()
    {   
        float hitOffset = isFlipped ? -offset_x : offset_x;
        
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(hitOffset, offset_y);
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0f);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out ObjetoQuebravel objetoQuebravel))
            {
                objetoQuebravel.Quebrar();
                continue; 
            }

            if (hit.isTrigger) continue;

            if (hit.CompareTag("Enemy") && hit.TryGetComponent(out Entity scriptInimigo))
            {
                scriptInimigo.TomarDano(danoGarra, "enemy");
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