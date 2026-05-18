using UnityEngine;

public class BossAI : MonoBehaviour
{
    public GameObject player;
    public float speed = 2.5f;
    private BossEntity entity;

    public bool flamethrowerRange;
    public bool drillRange;

    public int attackDamage = 30;
    public Vector2 drillHitboxSize = new Vector2(3f, 3f);
    public Vector2 drillHitboxOffset = new Vector2(2f, 0f);
    public Vector2 flamethrowerHitboxSize = new Vector2(6f, 3f);
    public Vector2 flamethrowerHitboxOffset = new Vector2(3f, 0f);

    public GameObject projectilePrefab;
    public Transform shootPoint;

    private Animator anim;
    private string state; 
    private string currentAttack;
    private bool isAttacking;
    private bool flipped;
    
    public GameObject drillVFXPrefab;
    public GameObject fireVFXPrefab;
    public Transform vfxSpawnPointDrill; 
    public Transform vfxSpawnPointFire;
    private bool VFXFlameSpawnned;
    
    private AudioSource audioSource;
    public AudioClip somDrillStart;
    public AudioClip somDrillAttack;
    public AudioClip somFlamethrower;
    public AudioClip somCannonLaunch;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        entity = GetComponent<BossEntity>();
        audioSource = GetComponent<AudioSource>();
        state = "choosing";
        currentAttack = "";
        isAttacking = false;
        flipped = false;
        flamethrowerRange = false;
        drillRange = false;
        VFXFlameSpawnned = false;
    }

    void Update()
    {
        if (entity.vidaAtual <= 0) state = "dead";
        if (state == "dead") return;

        LookAtPlayer();

        if (state == "choosing")
        {
            ChooseNextAttack();
        }
        else if (state == "moving_to_attack")
        {
            ExecuteAttackLogic();
        }
    }

    private void LookAtPlayer()
    {
        if (isAttacking) return;

        Vector2 direction = player.transform.position - transform.position;
        flipped = direction.x <= 0;
        int y_rotation = flipped ? 180 : 0;
        transform.eulerAngles = new Vector3(0, y_rotation, 0);
    }

    private void ChooseNextAttack()
    {
        int randomChoice = Random.Range(0, 100);

        if (randomChoice < 50)
        {
            currentAttack = "broca";
        }
        else if (randomChoice < 75)
        {
            currentAttack = "canhao";
        }   
        else
        {
            currentAttack = "canhao";
        }

        state = "moving_to_attack";
    }

    private void ExecuteAttackLogic()
    {
        if (currentAttack == "broca")
        {
            if (drillRange) StartAttack("range_broca");
            else MoveTowardsPlayer();
        }
        else if (currentAttack == "flamethrower")
        {
            if (flamethrowerRange) StartAttack("flamethrower");
            else MoveTowardsPlayer();
        }
        else if (currentAttack == "canhao")
        {
            StartAttack("canhao");
        }
    }

    private void MoveTowardsPlayer()
    {
        anim.SetTrigger("caminhada");
        
        Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void StartAttack(string triggerName)
    {
        isAttacking = true;
        state = "attacking";
        if (triggerName == "range_broca" && somDrillStart != null)
        {
            audioSource.PlayOneShot(somDrillStart);
        }
        anim.ResetTrigger("caminhada"); 
        anim.SetTrigger(triggerName);
    }

    public void CastDrillHitbox()
    {
        if (drillVFXPrefab != null && vfxSpawnPointDrill != null)
        {
            Vector3 spawnPos = vfxSpawnPointDrill.position;
            spawnPos.z = 0f; 
            
            Instantiate(drillVFXPrefab, spawnPos, vfxSpawnPointDrill.rotation);
        }
        if (somDrillAttack != null) audioSource.PlayOneShot(somDrillAttack);
        CastHitbox(drillHitboxOffset, drillHitboxSize, attackDamage- 10);
    }

    public void CastFlamethrowerHitbox()
    {
        if (fireVFXPrefab != null && vfxSpawnPointFire != null && !VFXFlameSpawnned)
        {
            Vector3 spawnPos = vfxSpawnPointFire.position;
            spawnPos.z = 0f;

            Instantiate(fireVFXPrefab, spawnPos, vfxSpawnPointFire.rotation);
            VFXFlameSpawnned = true;
            if (somFlamethrower != null) audioSource.PlayOneShot(somFlamethrower);
        }
        
        CastHitbox(flamethrowerHitboxOffset, flamethrowerHitboxSize, attackDamage+10);
    }

    public void FireCannon()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            Vector2 direction = (player.transform.position - shootPoint.position).normalized;
            if (somCannonLaunch != null) audioSource.PlayOneShot(somCannonLaunch);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 180f;
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.Euler(0f, 0f, angle));
        
            if (proj.TryGetComponent(out Rigidbody2D rb))
            {
                float projSpeed = 20f; 
                rb.linearVelocity = direction * projSpeed; 
            }
        }
    }

    public void FinishAttack()
    {
        isAttacking = false;
        currentAttack = "";
        state = "cooldown";
        anim.SetTrigger("cooldown");
        if (VFXFlameSpawnned)
        {
            VFXFlameSpawnned = false;
        }
    }

    public void FinishCooldown()
    {
        state = "choosing";
    }

    private void CastHitbox(Vector2 offset, Vector2 size, int dmg)
    {
        float actualOffsetX = flipped ? -Mathf.Abs(offset.x) : Mathf.Abs(offset.x);
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(actualOffsetX, offset.y);
        
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, size, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) 
            {
                if (hit.TryGetComponent(out Entity scriptInimigo))
                {
                    scriptInimigo.TomarDano(dmg, "player");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float drillX = flipped ? -Mathf.Abs(drillHitboxOffset.x) : Mathf.Abs(drillHitboxOffset.x);
        Vector2 drillPos = (Vector2)transform.position + new Vector2(drillX, drillHitboxOffset.y);
        Gizmos.DrawWireCube(drillPos, drillHitboxSize);

        Gizmos.color = new Color(1f, 0.5f, 0f);
        float flameX = flipped ? -Mathf.Abs(flamethrowerHitboxOffset.x) : Mathf.Abs(flamethrowerHitboxOffset.x);
        Vector2 flamePos = (Vector2)transform.position + new Vector2(flameX, flamethrowerHitboxOffset.y);
        Gizmos.DrawWireCube(flamePos, flamethrowerHitboxSize);
    }
    
}