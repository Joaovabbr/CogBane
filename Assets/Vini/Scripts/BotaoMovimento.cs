using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoMovimento : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direcao { Esquerda, Direita }
    public Direcao direcao;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerMovement == null) return;
        if (direcao == Direcao.Esquerda)
            playerMovement.mobileLeft = true;
        else
            playerMovement.mobileRight = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerMovement == null) return;
        if (direcao == Direcao.Esquerda)
            playerMovement.mobileLeft = false;
        else
            playerMovement.mobileRight = false;
    }
}
