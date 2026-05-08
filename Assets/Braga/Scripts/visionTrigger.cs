using UnityEngine;

public class visionTrigger : MonoBehaviour
{
    public EnemyAI enemyAI;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.visionClose = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.visionClose = false;
        }
    }
}
