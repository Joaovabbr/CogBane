using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoPocao : MonoBehaviour, IPointerDownHandler
{
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerInventory != null)
            playerInventory.UsarPocao();
    }
}
