using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameOverController : MonoBehaviour
{
    [Header("Elementos de Interface")]
    public TextMeshProUGUI textoFraseAleatoria; 

    [Header("Configurações")]
    public string nomeDoMenu = "MainMenu";
    
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

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (textoFraseAleatoria != null && frasesDeMorte.Length > 0)
        {
            int indiceSorteado = Random.Range(0, frasesDeMorte.Length);
            textoFraseAleatoria.text = frasesDeMorte[indiceSorteado];
        }
    }

    public void VoltarMenu()
    {
        Debug.Log("Botão de voltar ao menu foi clicado!");
        SceneManager.LoadScene(nomeDoMenu);
    }
}