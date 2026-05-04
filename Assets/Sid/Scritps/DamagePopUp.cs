using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Configurações da Animação")]
    public float velocidadeSubida = 2f;
    public float tempoDeVida = 1f;
    public float velocidadeSumir = 3f;

    private TextMeshPro textMesh;
    private Color corTexto;
    private float cronometro;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        cronometro = tempoDeVida;
    }

    // NOVO: Agora a função recebe o texto pronto (ex: "10" ou "+30") e a cor!
    public void Setup(string texto, Color cor)
    {
        textMesh.SetText(texto);
        textMesh.color = cor;
        corTexto = cor; // Salva a cor para podermos fazer o efeito de sumir depois
    }

    void Update()
    {
        transform.position += new Vector3(0, velocidadeSubida * Time.deltaTime, 0);
        cronometro -= Time.deltaTime;

        if (cronometro < tempoDeVida / 2f)
        {
            corTexto.a -= velocidadeSumir * Time.deltaTime; 
            textMesh.color = corTexto;
        }

        if (cronometro <= 0)
        {
            Destroy(gameObject);
        }
    }
}