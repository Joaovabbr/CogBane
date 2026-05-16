using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FullHealArea : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponent<PlayerInventory>();
        
        if (player != null)
        {
            player.EntrarAreaFullHeal();

            Debug.Log("Entre na área de Full Heal! Pressione R para curar.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponent<PlayerInventory>();
        
        if (player != null)
        {
            player.SairAreaFullHeal();
            
        }
    }
}