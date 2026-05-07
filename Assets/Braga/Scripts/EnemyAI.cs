using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{
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

    public bool dead;
    // controle de ataque
    private float CronometroAtack;
    public bool atackRange;
    
    // controle de animação
    private Animator anim;

    private float offset_x=1.5f;

    private bool isFlipped;

    private int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        state = "patrolling";
        cronometroPatrolStop = 0.0f;
        CronometroAtack = 0.0f;
        dead = false;
        damage = 30;
    }

    // Update is called once per frame
    void Update()
    {  
        // define state
        print(Vector2.Distance(transform.position, currentPoint.position) < 0.3f);
        print(currentPoint);
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
                    print("entrei aqui B");
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
                    print("entrei aqui A");
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
              CastAttackHitbox();  
            } 
           CronometroAtack += Time.deltaTime;
           if (CronometroAtack >= 1)
           {
               state = "running";
               CronometroAtack = 0;
           }
            
        }
        else if (state == "dead")
        {
            if (!dead) anim.SetTrigger("death");
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
        if (isFlipped) offset_x = -offset_x;
        Vector2 hitboxPos = (Vector2)transform.position + new Vector2(offset_x, 0);
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitboxPos, 0.5f);
        foreach (Collider2D hit in hits)
        {
            // Se acertou o player que você já tinha salvo no trigger
            if (hit.gameObject == player) 
            {
                // Você AINDA precisa usar o GetComponent para acessar a função no script
                player.GetComponent<PlayerEntity>().TomarDano(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (isFlipped) offset_x = -offset_x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(offset_x, 0), 0.5f);
    }
}
