using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float speed;

    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance  = Vector2.Distance(transform.position, player.transform.position );
        Vector2 direction = player.transform.position - transform.position;
        

        if (distance < 6)
        {
            transform.position  = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
