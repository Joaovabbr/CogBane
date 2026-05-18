using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{
    [Header("Áudio de Ataque")]
    public AudioSource audioSource;
    public AudioClip somAtaqueLobo;
    [Range(0f, 1f)] 
    public float volumeAtaque = 0.4f; 

    private GameObject player;
    public float patrolSpeed;
    public float followingSpeed;
    private Rigidbody2D rb;
    public GameObject pointA;
    public GameObject pointB;
    public GameObject returnPoint;
    private Transform currentPoint;
    private string state;
    public float patrolStopDuration;
    private float distance;
    private float cronometroPatrolStop;
    private bool isStopped;
    public bool close;
    public bool visionClose;
    private bool patrolRange;
    public WolfEntity wolfEntity;
    
    public bool dead;
    // controle de ataque
    private float CronometroAtack;
    public bool atackRange;
    
    // controle de animação
    private Animator anim;

    private float offset_x;
    private bool isFlipped;
    private int damage;

    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        state = "patrolling";
        cronometroPatrolStop = 0.0f;
        CronometroAtack = 0.0f;
        dead = false;
        damage = 15;
        wolfEntity = this.GetComponent<WolfEntity>();

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {  
        // define state
        if (wolfEntity.vidaAtual == 0) state = "dead" ;
        if (close && state != "dead") state = "running";
        if (close == false && state != "dead") state = "patrolling";    
        if (state == "running" &&  atackRange && state != "dead") state = "attacking";
        
        // ação de state
        if (state == "patrolling")
        {
            Vector2 direction = currentPoint.transform.position - transform.position;
            if (direction.x > 0) transform.eulerAngles = new Vector3(0, 0, 0);
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            transform.position  = Vector2.MoveTowards(this.transform.position, currentPoint.transform.position, patrolSpeed * Time.deltaTime);
            anim.SetFloat("speed", patrolSpeed);
            if (!isStopped)
            {
                cronometroPatrolStop = 0.0f;
            }
            
            if (Math.Abs(transform.position.x - currentPoint.position.x) < 0.5f && currentPoint == pointB.transform)
            {
                cronometroPatrolStop +=  Time.deltaTime;
                patrolSpeed = 0;
                anim.SetFloat("speed", patrolSpeed);
                isStopped = true;
                if (cronometroPatrolStop >= patrolStopDuration)
                {
                    patrolSpeed = 0.75f;
                    anim.SetFloat("speed", patrolSpeed);
                    isStopped = false;
                    currentPoint = pointA.transform;
                }
                
            }
            if (Math.Abs(transform.position.x - currentPoint.position.x) < 0.5f && currentPoint == pointA.transform )
            {
                cronometroPatrolStop +=  Time.deltaTime;
                patrolSpeed = 0;
                anim.SetFloat("speed", patrolSpeed);
                isStopped = true;
                if (cronometroPatrolStop >= patrolStopDuration)
                {
                    patrolSpeed = 0.75f;
                    anim.SetFloat("speed", patrolSpeed);
                    isStopped = false;
                    currentPoint = pointB.transform;
                }
            }
        }
        else if (state == "attacking")
        {
            if (CronometroAtack == 0)
            {
                anim.SetTrigger("attack");
              
                if (audioSource != null && somAtaqueLobo != null)
                {
                    audioSource.PlayOneShot(somAtaqueLobo, volumeAtaque);
                }
            } 
            CronometroAtack += Time.deltaTime;
            if (CronometroAtack >= 0.5)
            {
                state = "running";
                CronometroAtack = 0;
            }
        }
        else if (state == "dead")
        {
            followingSpeed = 0;
            patrolSpeed = 0;
            dead = true;
        }
        else
        {
            CronometroAtack = 0;
            Vector2 direction = player.transform.position - transform.position;
            if (direction.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFlipped = false;
            }
            if  (direction.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isFlipped = true;
            }
            transform.position  = Vector2.MoveTowards(this.transform.position, player.transform.position, followingSpeed * Time.deltaTime);
            anim.SetFloat("speed", followingSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            close = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !visionClose)
        {
            close = false;
            player = null;
        }
    }

    public void CastAttackHitbox()
    {   
        if (isFlipped) offset_x = -2f;
        else offset_x = 2f;
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(offset_x, 0);
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitboxPos, 1f);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == player) 
            {
                player.GetComponent<PlayerEntity>().TomarDano(damage, "player" );
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        float gizmoOffset = isFlipped ? -2f : 2f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(gizmoOffset, 0), 1f);
    }
}