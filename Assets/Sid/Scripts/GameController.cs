using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instancia;

    [Header("Referências da Interface (HUD)")]
    public Image barraDeVida;
    public TextMeshProUGUI textoContadorPocoes;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AtualizarBarraDeVida(float vidaAtual, float vidaMaxima)
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = vidaAtual / vidaMaxima;
        }
        else
        {
            Debug.LogWarning("Aviso: A Imagem da Barra de Vida não foi conectada no UIManager!");
        }
    }

    public void AtualizarContadorPocoes(int pocoesAtuais)
    {
        if (textoContadorPocoes != null)
        {
            textoContadorPocoes.text = pocoesAtuais.ToString();
        }
        else
        {
            Debug.LogWarning("Aviso: O Texto das Poções não foi conectado no UIManager!");
        }
    }
}