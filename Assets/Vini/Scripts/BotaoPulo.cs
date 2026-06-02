using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoPulo : MonoBehaviour, IPointerDownHandler
{
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerMovement != null)
            playerMovement.mobilePulo = true;
    }
}
