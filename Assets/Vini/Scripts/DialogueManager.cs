using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instancia;

    [Header("UI do Diálogo")]
    public GameObject dialogueBox;
    public GameObject botoesHUD;
    public Image retrato;
    public TextMeshProUGUI nomeTexto;
    public TextMeshProUGUI dialogoTexto;
    public GameObject indicadorContinuar;

    [Header("Configurações")]
    public float velocidadeTexto = 0.05f;

    // --- NOVAS VARIÁVEIS DE ÁUDIO AQUI ---
    [Header("Áudio do Diálogo")]
    public AudioSource audioSourceDialogo;
    public AudioClip somDeLetra;
    // -------------------------------------

    private bool dialogoAtivo = false;
    private bool textoCompleto = false;
    private Coroutine digitandoCoroutine;
    private string[] dialogosAtuais;
    private Sprite[] retratosAtuais;
    private string[] nomesAtuais;
    private int indiceAtual = 0;
    private UnityAction onDialogoFinalizado; 

    void Awake()
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

    void Start()
    {
        FecharDialogo();
    }

    void Update()
    {
        if (!dialogoAtivo) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!textoCompleto)
            {
                CompletarTextoInstantaneamente();
            }
            else
            {
                ProximoDialogo();
            }
        }
    }

    public void IniciarDialogo(string[] dialogos, Sprite[] retratos, string[] nomes, UnityAction onFinalizado = null)
    {
        dialogoAtivo = true;
        dialogosAtuais = dialogos;
        retratosAtuais = retratos;
        nomesAtuais = nomes;
        indiceAtual = 0;
        onDialogoFinalizado = onFinalizado; 

        dialogueBox.SetActive(true);
        if (botoesHUD != null) botoesHUD.SetActive(false);
        retrato.gameObject.SetActive(true);
        nomeTexto.gameObject.SetActive(true);
        dialogoTexto.gameObject.SetActive(true);

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) 
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            
            Animator anim = player.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetFloat("Speed", 0f);
            }
            
            player.enabled = false;
        }

        PlayerCombat combat = FindObjectOfType<PlayerCombat>();
        if (combat != null) combat.enabled = false;

        MostrarDialogo();
    }

    void MostrarDialogo()
    {
        if (indiceAtual >= dialogosAtuais.Length)
        {
            FecharDialogo();
            return;
        }

        if (retratosAtuais != null && indiceAtual < retratosAtuais.Length && retratosAtuais[indiceAtual] != null)
        {
            retrato.sprite = retratosAtuais[indiceAtual];
            retrato.gameObject.SetActive(true);
        }
        else
        {
            retrato.gameObject.SetActive(false);
        }

        if (nomesAtuais != null && indiceAtual < nomesAtuais.Length)
        {
            nomeTexto.text = nomesAtuais[indiceAtual];
        }

        textoCompleto = false;
        if (indicadorContinuar != null) indicadorContinuar.SetActive(false);

        if (digitandoCoroutine != null)
        {
            StopCoroutine(digitandoCoroutine);
        }

        digitandoCoroutine = StartCoroutine(DigitarTexto(dialogosAtuais[indiceAtual]));
    }

    IEnumerator DigitarTexto(string texto)
    {
        dialogoTexto.text = "";

        foreach (char letra in texto.ToCharArray())
        {
            dialogoTexto.text += letra;
            
            // --- INJEÇÃO DO EFEITO SONORO AQUI ---
            // Toca o som para caracteres que não sejam espaços em branco para um ritmo melhor
            if (somDeLetra != null && audioSourceDialogo != null && letra != ' ')
            {
                audioSourceDialogo.pitch = Random.Range(0.9f, 1.1f);
                audioSourceDialogo.PlayOneShot(somDeLetra);
            }
            // -------------------------------------

            yield return new WaitForSeconds(velocidadeTexto);
        }

        textoCompleto = true;
        if (indicadorContinuar != null) indicadorContinuar.SetActive(true);
    }

    void CompletarTextoInstantaneamente()
    {
        if (digitandoCoroutine != null)
        {
            StopCoroutine(digitandoCoroutine);
        }

        dialogoTexto.text = dialogosAtuais[indiceAtual];
        textoCompleto = true;
        if (indicadorContinuar != null) indicadorContinuar.SetActive(true);
    }

    void ProximoDialogo()
    {
        indiceAtual++;
        MostrarDialogo();
    }

    void FecharDialogo()
    {
        dialogoAtivo = false;
        
        dialogueBox.SetActive(false);
        retrato.gameObject.SetActive(false);
        nomeTexto.gameObject.SetActive(false);
        dialogoTexto.gameObject.SetActive(false);
        if (indicadorContinuar != null) indicadorContinuar.SetActive(false);
        if (botoesHUD != null) botoesHUD.SetActive(true);

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) 
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            
            player.enabled = true;
        }

        PlayerCombat combat = FindObjectOfType<PlayerCombat>();
        if (combat != null) combat.enabled = true;
        
        if (onDialogoFinalizado != null)
        {
            onDialogoFinalizado.Invoke();
            onDialogoFinalizado = null;
        }
    }

    public bool EstaAtivo()
    {
        return dialogoAtivo;
    }
}