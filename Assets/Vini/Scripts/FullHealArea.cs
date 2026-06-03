using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FullHealArea : MonoBehaviour
{
    [Header("Interface Mobile")]
    public GameObject botaoDescansoMobile;

    private PlayerInventory playerNaArea;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        
        // Esconde o botão ao iniciar o jogo
        if (botaoDescansoMobile != null) 
        {
            botaoDescansoMobile.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponent<PlayerInventory>();
        
        if (player != null)
        {
            playerNaArea = player; 
            player.EntrarAreaFullHeal(); 

            if (botaoDescansoMobile != null) 
            {
                botaoDescansoMobile.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerInventory player = other.GetComponent<PlayerInventory>();
        
        if (player != null)
        {
            player.SairAreaFullHeal(); 
            playerNaArea = null; 
            
            if (botaoDescansoMobile != null) 
            {
                botaoDescansoMobile.SetActive(false);
            }
        }
    }
    public void CurarPeloMobile()
    {
        if (playerNaArea != null)
        {
            playerNaArea.UsarFullHeal(); 
        }
    }
}