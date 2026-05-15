using UnityEngine;

public class werewolfAI : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private string state;
    private Animator anim;
    private werewollfEntity werewolf;

    private float cronometroAtack;
    private int howlCount = 4;
    private bool howl;
    public bool atackRange;

    private int danoAtaque = 20;
    public Vector2 hitboxSize = new Vector2(2f, 2.5f);
    public float howlHitboxRadius = 2f;  
    public float offset_x;
    public float offset_y;
    
    public bool flipped;

    void Start()
    {
        anim = GetComponent<Animator>();
        werewolf = GetComponent<werewollfEntity>();
        state = "running";
        cronometroAtack = 0;
        atackRange = false;
        howl = false;
        speed = 6;
    }

    void Update()
    {
        print("Estado: "+state);
        print("Boleano do uivo: "+howl);
        float razaoVida = werewolf.vidaAtual / werewolf.vidaMaxima * 100;
        
        if ((razaoVida <= 75 && howlCount == 4) || (razaoVida <= 50 && howlCount == 3) || (razaoVida <= 25 && howlCount == 2) || (razaoVida <= 5 && howlCount == 1))
        {
            howl = true;
            howlCount--;
            state = "attacking"; 
            cronometroAtack = 0;
        }

        if (razaoVida <= 0) state = "dead";
        
        if (state != "attacking" && state != "dead")
        {
            if (atackRange) state = "attacking";
            else state = "running";
        }

        if (state == "dead") return;

        if (state == "running")
        {
            speed = 6;
            Vector2 direction = player.transform.position - transform.position;
            flipped = direction.x <= 0; 
            
            int y_rotation = flipped ? 180 : 0;
            transform.eulerAngles = new Vector3(0, y_rotation, 0);
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (state == "attacking")
        {
            if (cronometroAtack == 0)
            {
                speed = 0;
                if (!howl)
                {
                    anim.SetTrigger("attacking");
                }
                else
                {
                    werewolf.invencible = true;
                    anim.SetTrigger("howl");
                }
            }
            
            cronometroAtack += Time.deltaTime;
            
            if (cronometroAtack >= 1 && !howl)
            {
                state = "running";
                cronometroAtack = 0;
            }
        }
    }

    public void CastAttackHitbox()
    {
        print("chamou attack");
        offset_x = flipped ? -Mathf.Abs(offset_x) : Mathf.Abs(offset_x);
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(offset_x, offset_y);
        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) 
            {
                if (hit.TryGetComponent(out Entity scriptInimigo))
                {
                    scriptInimigo.TomarDano(danoAtaque, "player");
                }
            }
        }
    }
    
    public void CastHowlHitbox()
    {   
        print("chamou howl");
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(0, 2);
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitboxPos, howlHitboxRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) 
            {
                if (hit.TryGetComponent(out Entity scriptInimigo))
                {
                    scriptInimigo.TomarDano(danoAtaque, "player");
                }
            }
        }
    }
    
    public void FinishHowl()
    {
        print("Finalizou Howl");
        howl = false;
        werewolf.invencible = false;
        cronometroAtack = 0;
        state = "running";
    }

    private void OnDrawGizmosSelected()
    {
        float gizmoOffset = flipped ? -Mathf.Abs(offset_x) : Mathf.Abs(offset_x);
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(gizmoOffset, offset_y);
        Vector2 HowlHitboxPos = (Vector2)transform.position + new Vector2(0, 2);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitboxPos, hitboxSize); 
        Gizmos.DrawWireSphere(HowlHitboxPos, howlHitboxRadius); 
    }
}