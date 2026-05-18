using UnityEngine;
using UnityEngine.SceneManagement; 

public class BossDialogueMrM : MonoBehaviour
{
    [Header("Diálogo Antes da Luta")]
    [TextArea(3, 10)]
    public string[] dialogosIntro;
    public Sprite[] retratosIntro;
    public string[] nomesIntro;

    [Header("Diálogo Após Derrotar")]
    [TextArea(3, 10)]
    public string[] dialogosDerrota;
    public Sprite[] retratosDerrota;
    public string[] nomesDerrota;

    [Header("Configurações")]
    public float distanciaParaIniciar = 5f;
    public string nomeCenaFinal = "GameClear"; 
    
    private bool dialogoIntroMostrado = false;
    private bool dialogoDerrotaMostrado = false;
    private bool lutaIniciada = false;

    private Transform player;
    private BossAI bossAI;
    private BossEntity bossEntity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossAI = GetComponent<BossAI>();
        bossEntity = GetComponent<BossEntity>();

        if (bossAI != null)
        {
            bossAI.enabled = false;
        }
    }

    void Update()
    {
        if (!dialogoIntroMostrado && !DialogueManager.Instancia.EstaAtivo())
        {
            float distancia = Vector2.Distance(transform.position, player.position);
            
            if (distancia <= distanciaParaIniciar)
            {
                IniciarDialogoIntro();
            }
        }

        if (lutaIniciada && !dialogoDerrotaMostrado && bossEntity != null && bossEntity.vidaAtual <= 0)
        {
            IniciarDialogoDerrota();
        }
    }

    void IniciarDialogoIntro()
    {
        dialogoIntroMostrado = true;

        if (DialogueManager.Instancia != null && dialogosIntro.Length > 0)
        {
            DialogueManager.Instancia.IniciarDialogo(dialogosIntro, retratosIntro, nomesIntro, IniciarLuta);
        }
        else
        {
            IniciarLuta();
        }
    }

    void IniciarLuta()
    {
        lutaIniciada = true;
        
        if (bossAI != null)
        {
            bossAI.enabled = true;
        }

        Debug.Log("Luta contra o boss iniciada!");
    }

    void IniciarDialogoDerrota()
    {
        dialogoDerrotaMostrado = true;

        if (DialogueManager.Instancia != null && dialogosDerrota.Length > 0)
        {
            DialogueManager.Instancia.IniciarDialogo(dialogosDerrota, retratosDerrota, nomesDerrota, CarregarCenaFinal);
        }
        else
        {
            CarregarCenaFinal();
        }
    }

    void CarregarCenaFinal()
    {
        Debug.Log("Carregando cena final: " + nomeCenaFinal);
        SceneManager.LoadScene(nomeCenaFinal);
    }
}