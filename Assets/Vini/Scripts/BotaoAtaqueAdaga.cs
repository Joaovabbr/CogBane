using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoAtaqueAdaga : MonoBehaviour, IPointerDownHandler
{
    private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCombat != null)
            playerCombat.mobileDagger = true;
    }
}
