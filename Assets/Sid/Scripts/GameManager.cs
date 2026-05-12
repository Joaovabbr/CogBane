using UnityEngine;
using UnityEngine.SceneManagement; // Adicionamos isso para trocar de cena

public class GameManager : MonoBehaviour
{
    public StatusJogadorSO statusDamon;

    // Assim que o Menu Principal abre, o jogo já reseta a vida e as poções
    void Awake()
    {
        if (statusDamon != null)
        {
            statusDamon.ResetarParaNovoJogo();
        }
    }
    public void IniciarJogo()
    {
        SceneManager.LoadScene("Floresta 1"); 
    }
}