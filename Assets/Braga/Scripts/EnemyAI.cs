using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    public float patrolSpeed;
    public float followingSpeed;
    private Rigidbody2D rb;
    public GameObject pointA;
    public GameObject pointB;
    private Transform currentPoint;
    private string state;
    public float patrolStopDuration;
    private float distance;
    private float cronometroPatrolStop;
    private bool isStopped;
    private bool close;

    public bool dead;
    // controle de ataque
    private float CronometroAtack;
    public static bool atackRange;
    
    // controle de animação
    private Animator anim;
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
    }

    // Update is called once per frame
    void Update()
    {  
        // define state
        if (close && state != "dead") state = "running";
        if (close == false && state != "dead") state = "patrolling";
        if (state == "running" &&  atackRange && state != "dead") state = "attacking";
        
        // ação de state
        if (state == "patrolling")
        {
            anim.SetFloat("speed", patrolSpeed);
            if (!isStopped)
            {
                cronometroPatrolStop = 0.0f;
            }
            if (currentPoint == pointB.transform)
            {
                rb.linearVelocity = new Vector2(patrolSpeed, 0);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-patrolSpeed, 0);
                transform.eulerAngles = new Vector3(0, 180f, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
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
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
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
           if (CronometroAtack == 0) anim.SetTrigger("attack");
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
            Vector2 direction = player.transform.position - transform.position;
            if (direction.x > 0) transform.eulerAngles = new Vector3(0, 0, 0);
            if  (direction.x < 0) transform.eulerAngles = new Vector3(0, 180, 0);
            transform.position  = Vector2.MoveTowards(this.transform.position, player.transform.position, followingSpeed * Time.deltaTime);
            anim.SetFloat("speed", followingSpeed);
        }
        
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            close = true;
            player = other.gameObject;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            close = false;
            player = null;
        }
    }
}
