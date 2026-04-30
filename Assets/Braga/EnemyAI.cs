using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float patrolSpeed;
    public float followingSpeed;
    private Rigidbody2D rb;
    public GameObject pointA;
    public GameObject pointB;
    private Transform currentPoint;
    private bool isPatrolling;
    public float patrolStopDuration;
    private float cronometroPatrolStop;
    private bool isStopped;
    

    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
        isPatrolling = true;
        cronometroPatrolStop = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {   
        distance  = Vector2.Distance(transform.position, player.transform.position );
        if (distance <= 4) isPatrolling = false;
        if (distance > 4) isPatrolling = true;
        if (isPatrolling)
        {
            if (!isStopped)
            {
                cronometroPatrolStop = 0.0f;
            }
            if (currentPoint == pointB.transform)
            {
                rb.linearVelocity = new Vector2(patrolSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-patrolSpeed, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            {
                
                cronometroPatrolStop +=  Time.deltaTime;
                patrolSpeed = 0;
                print(cronometroPatrolStop + " B");
                isStopped = true;
                if (cronometroPatrolStop >= patrolStopDuration)
                {
                    patrolSpeed = 0.75f;
                    isStopped = false;
                    currentPoint = pointA.transform;
                }
                
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                
                cronometroPatrolStop +=  Time.deltaTime;
                patrolSpeed = 0;
                print(cronometroPatrolStop+ " A");
                isStopped = true;
                if (cronometroPatrolStop >= patrolStopDuration)
                {
                    patrolSpeed = 0.75f;
                    isStopped = false;
                    currentPoint = pointB.transform;
                }
            }
        }
        else
        {
            Vector2 direction = player.transform.position - transform.position;
            transform.position  = Vector2.MoveTowards(this.transform.position, player.transform.position, followingSpeed * Time.deltaTime);
        }
        
        
    }
}
