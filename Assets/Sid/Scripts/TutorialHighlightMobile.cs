using System.Collections;
using UnityEngine;
using UnityEngine.UI; // <-- Obrigatório para podermos alterar a cor da imagem

[RequireComponent(typeof(Collider2D))]
public class TutorialHighlightMobile : MonoBehaviour
{
    [Header("Qual botão da UI deve piscar?")]
    public Transform botaoMobile; 
    
    [Header("Configurações do Efeito")]
    public float velocidadePulsar = 1.5f; 
    public float tamanhoMaximo = 0.15f; 
    public Color corDestaque = Color.red; 

    private bool isPulsing = false;
    private Vector3 escalaOriginal;
    
    private Image imagemBotao;
    private Color corOriginal;

    void Start()
    {
        if (botaoMobile != null) 
        {
            escalaOriginal = botaoMobile.localScale;
            
            imagemBotao = botaoMobile.GetComponent<Image>();
            if (imagemBotao != null)
            {
                corOriginal = imagemBotao.color; 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !isPulsing && botaoMobile != null)
        {
            isPulsing = true;
            StartCoroutine(PulsarBotao());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && isPulsing)
        {
            PararEfeito();
        }
    }

    public void PararEfeito()
    {
        isPulsing = false;
        if (botaoMobile != null)
        {
            botaoMobile.localScale = escalaOriginal; 
        }
        if (imagemBotao != null)
        {
            imagemBotao.color = corOriginal; 
        }
    }

    IEnumerator PulsarBotao()
    {
        while (isPulsing)
        {
            float tempo = Mathf.PingPong(Time.time * velocidadePulsar, 1f);

            float scale = 1f + (tempo * tamanhoMaximo);
            botaoMobile.localScale = escalaOriginal * scale;
            
            if (imagemBotao != null)
            {
                imagemBotao.color = Color.Lerp(corOriginal, corDestaque, tempo);
            }
            
            yield return null; 
        }
    }
}