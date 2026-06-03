using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoAtaqueGarra : MonoBehaviour, IPointerDownHandler
{
    public StatusJogadorSO statusDamon;

    private PlayerCombat playerCombat;
    private Vector3 escalaOriginal; 

    void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
        
        escalaOriginal = transform.localScale; 
        
        AtualizarVisibilidade();
    }

    void Update()
    {
        AtualizarVisibilidade();
    }

    void AtualizarVisibilidade()
    {
        if (statusDamon != null)
        {
            if (statusDamon.garraDesbloqueada)
            {
                transform.localScale = escalaOriginal; 
            }
            else
            {
                transform.localScale = Vector3.zero; 
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (statusDamon != null && !statusDamon.garraDesbloqueada) return;

        if (playerCombat != null)
            playerCombat.mobileGarra = true;
    }
}