using UnityEngine;

public class playerMovement : MonoBehaviour
{   
    
    public float speed;
    public float jump;

    private float Move;

    public Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        
        rb.linearVelocity = new Vector2(Move*speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jump));
        }
        
    }
}
