using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoAtaqueBesta : MonoBehaviour, IPointerDownHandler
{
    private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCombat != null)
            playerCombat.mobileBesta = true;
    }
}
