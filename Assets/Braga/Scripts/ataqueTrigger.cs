using UnityEngine;

namespace Braga.Scripts
{
    public class ataqueTrigger : MonoBehaviour
    {
        public EnemyAI enemyAI;
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
        }
    }
}
