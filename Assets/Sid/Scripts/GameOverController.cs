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
    
    [Header("Áudio de Escrita")]
    public AudioSource audioSource;
    public AudioClip somLetra;

    [Header("Áudio da Voz de Game Over")]
    public AudioClip somVozGameOver;
    [Range(0f, 1f)]
    public float volumeVoz = 1f;

    [Header("Áudio do Botão")]
    public AudioClip somCliqueBotao;

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
        
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (somVozGameOver != null)
        {
            AudioSource.PlayClipAtPoint(somVozGameOver, Camera.main.transform.position, volumeVoz);
        }

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
            
            if (i > 0)
            {
                char letraAtual = frase[i - 1];

                if (letraAtual != ' ' && audioSource != null && somLetra != null)
                {
                    audioSource.pitch = Random.Range(0.85f, 1.15f);
                    audioSource.PlayOneShot(somLetra);
                }
            }
            
            yield return new WaitForSecondsRealtime(tempoEntreLetras);
        }
    }

    public void VoltarMenu()
    {
        Debug.Log("Botão de voltar ao menu foi clicado!");
        StartCoroutine(TocarSomEVoltar(nomeDoMenu));
    }

    private IEnumerator TocarSomEVoltar(string nomeCena)
    {
        if (audioSource != null && somCliqueBotao != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(somCliqueBotao);
            yield return new WaitForSecondsRealtime(somCliqueBotao.length);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.2f);
        }

        SceneManager.LoadScene(nomeCena);
    }
}