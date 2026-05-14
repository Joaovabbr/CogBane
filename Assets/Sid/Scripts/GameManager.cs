using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public StatusJogadorSO statusDamon;

    void Awake()
    {
        // Garante que o arquivo de status comece com os valores padrão no menu principal
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