using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
using System.Collections; 

public class GameOverController : MonoBehaviour
{
    [Header("Elementos de Interface")]
    public TextMeshProUGUI textoFraseAleatoria; 

    [Header("Configurações")]
    public string nomeDoMenu = "MainMenu";
    public float tempoEntreLetras = 0.05f; 
    
    [Header("Frases Temáticas")]
    [TextArea(2, 3)]
    public string[] frasesDeMorte = new string[]
    {
        "A CAÇADA TERMINOU.",
        "GEAR TOWN COBRA SEU TRIBUTO DE SANGUE.",
        "ENGRENAGENS QUEBRADAS, CARNE RASGADA.",
        "MAIS UMA PEÇA DESCARTADA NA FÁBRICA DA NOITE.",
        "A LUA CARMESIM REFLETE SEU FRACASSO.",
        "SUA LÂMINA CEGOU, E O SEU TEMPO ACABOU.",
        "OS BECOS DEVORAM OS FRACOS.",
        "A ENGRENAGEM PARA. A CARNE APODRECE.",
        "O SANGUE FOI COBRADO PELAS RUAS DE PEDRA.",
        "GEAR TOWN NÃO DEIXA SOBREVIVENTES.",
        "SUA LÂMINA NÃO FOI SUFICIENTE PARA A ESCURIDÃO.",
        "A MÁQUINA CONTINUA A GIRAR. VOCÊ, NÃO.",
        "AS SOMBRAS REIVINDICAM MAIS UM CAÇADOR.",
        "FERRUGEM E OSSOS. O DESTINO DE TODOS AQUI.",
        "O ECO DOS SEUS PASSOS FINALMENTE SILENCIOU.",
        "AS POÇÕES QUEBRARAM. SUA SORTE TAMBÉM."
    };

    private Coroutine rotinaDigitacao;

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (textoFraseAleatoria != null && frasesDeMorte.Length > 0)
        {
            string fraseEscolhida = frasesDeMorte[Random.Range(0, frasesDeMorte.Length)];
            
            if (rotinaDigitacao != null) StopCoroutine(rotinaDigitacao);
            rotinaDigitacao = StartCoroutine(DigitarLetraPorLetra(fraseEscolhida));
        }
    }

    private IEnumerator DigitarLetraPorLetra(string frase)
    {
        textoFraseAleatoria.text = frase;
        textoFraseAleatoria.maxVisibleCharacters = 0;

        for (int i = 0; i <= frase.Length; i++)
        {
            textoFraseAleatoria.maxVisibleCharacters = i;
            
            yield return new WaitForSecondsRealtime(tempoEntreLetras);
        }
    }

    public void VoltarMenu()
    {
        Debug.Log("Botão de voltar ao menu foi clicado!");
        SceneManager.LoadScene(nomeDoMenu);
    }
}