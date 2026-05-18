using UnityEngine;

public class werewolfAttack : MonoBehaviour
{
    public werewolfAI enemyAI;
    private void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag("Player")){
            enemyAI.atackRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)  
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.atackRange = false;
        }
    }}
