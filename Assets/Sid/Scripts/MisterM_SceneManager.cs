using UnityEngine;
using UnityEngine.UI;

public class MisterM_SceneManager : MonoBehaviour
{
    [Header("Referências do Chefe")]
    public Entity bossScript; 
    
    [Header("Interface do Boss")]
    public GameObject canvasBossHUD; 
    public Image barraDeVidaFill; 

    private bool bossJaMorreu = false;

    void Start()
    {
        if (bossScript != null && bossScript.vidaAtual > 0)
        {
            if (canvasBossHUD != null) canvasBossHUD.SetActive(true);
        }
        else
        {
            FinalizarCenaInimiga();
        }
    }

    void Update()
    {
        if (bossJaMorreu) return;

        if (bossScript != null)
        {
            if (barraDeVidaFill != null)
            {
                barraDeVidaFill.fillAmount = bossScript.vidaAtual / bossScript.vidaMaxima;
            }

            if (bossScript.vidaAtual <= 0)
            {
                FinalizarCenaInimiga();
            }
        }
        else
        {
            FinalizarCenaInimiga();
        }
    }

    private void FinalizarCenaInimiga()
    {
        if (bossJaMorreu) return; 

        bossJaMorreu = true;
        
        if (canvasBossHUD != null) canvasBossHUD.SetActive(false); 

        Debug.Log("Mr. M foi derrotado! [Espaço reservado para carregar tela de vitória ou créditos]");

        // =========================================================================
        // TODO: IMPLEMENTAR AS LÓGICAS DE VITÓRIA / FIM DE JOGO DAQUI PARA BAIXO
        // Exemplos futuros:
        // UnityEngine.SceneManagement.SceneManager.LoadScene("TelaVitoria");
        // ou desativar os controles do jogador e rodar animação de créditos.
        // =========================================================================
    }
}