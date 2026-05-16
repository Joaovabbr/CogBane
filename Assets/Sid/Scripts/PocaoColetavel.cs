using UnityEngine;

public class PocaoColetavel : MonoBehaviour
{
    [Header("Persistência")]
    public StatusJogadorSO statusDamon;
    [Tooltip("RG da poção. Clique nos 3 pontinhos do componente e vá em 'Gerar ID'")]
    public string idUnico;

    [Header("Animação de Flutuar")]
    public float velocidadeFlutuar = 2f;
    public float alturaFlutuar = 0.2f;
    private Vector2 posicaoInicial;

    [Header("Áudio de Coleta")]
    public AudioClip somColeta;
    [Range(0f, 2f)]
    public float volumeColeta = 2f; // Controle de volume direto no Inspector

    void Start()
    {
        if (statusDamon != null && statusDamon.itensColetados.Contains(idUnico))
        {
            Destroy(gameObject);
            return; 
        }

        posicaoInicial = transform.position;
    }

    void Update()
    {
        float novoY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuar) * alturaFlutuar;
        transform.position = new Vector2(posicaoInicial.x, novoY);
    }

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Player"))
        {
            PlayerInventory inventario = colisao.GetComponent<PlayerInventory>();
            
            if (inventario != null)
            {
                inventario.AdicionarPocao(1); 

                if (statusDamon != null && !string.IsNullOrEmpty(idUnico))
                {
                    if (!statusDamon.itensColetados.Contains(idUnico))
                    {
                        statusDamon.itensColetados.Add(idUnico);
                    }
                }

                // Toca o som no local onde a poção estava antes de ser destruída
                if (somColeta != null)
                {
                    
                    AudioSource.PlayClipAtPoint(somColeta, Camera.main.transform.position, volumeColeta);
                }

                Destroy(gameObject); 
            }
        }
    }

    [ContextMenu("Gerar ID Único Automático")]
    private void GerarIDAutomatico()
    {
        idUnico = System.Guid.NewGuid().ToString();
    }
}