using UnityEngine;

public class BossAttackDrill : MonoBehaviour
{
    public BossAI boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.drillRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.drillRange = false;
        }
    }
}