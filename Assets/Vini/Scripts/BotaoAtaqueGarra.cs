using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoAtaqueGarra : MonoBehaviour, IPointerDownHandler
{
    public StatusJogadorSO statusDamon;

    private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
        AtualizarVisibilidade();
    }

    void Update()
    {
        AtualizarVisibilidade();
    }

    void AtualizarVisibilidade()
    {
        if (statusDamon != null)
            gameObject.SetActive(statusDamon.garraDesbloqueada);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCombat != null)
            playerCombat.mobileGarra = true;
    }
}
