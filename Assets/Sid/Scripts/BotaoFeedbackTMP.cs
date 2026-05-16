using UnityEngine;
using TMPro; 
using UnityEngine.EventSystems; 

public class BotaoFeedbackTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textoDoBotao;
    
    [Header("Configuração de Cores")]
    public Color corNormal = Color.white;
    public Color corHover = new Color(0.5f, 0f, 0f, 1f); 

    void Awake()
    {
        // Mágica aqui: Agora ele procura o texto nele mesmo OU nos filhos!
        textoDoBotao = GetComponentInChildren<TextMeshProUGUI>();
        
        if (textoDoBotao != null)
        {
            textoDoBotao.color = corNormal;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textoDoBotao != null)
        {
            textoDoBotao.color = corHover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textoDoBotao != null)
        {
            textoDoBotao.color = corNormal;
        }
    }

    void OnDisable()
    {
        if (textoDoBotao != null)
        {
            textoDoBotao.color = corNormal;
        }
    }
}