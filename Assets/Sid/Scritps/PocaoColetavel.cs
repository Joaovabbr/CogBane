using UnityEngine;

public class PocaoColetavel : MonoBehaviour
{
    [Header("Animação de Flutuar")]
    public float velocidadeFlutuar = 2f;
    public float alturaFlutuar = 0.2f;
    private Vector2 posicaoInicial;

    void Start()
    {
        // Salva a posição original para saber de onde subir e descer
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Faz o objeto flutuar usando uma onda senoidal (sobe e desce suavemente)
        float novoY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuar) * alturaFlutuar;
        transform.position = new Vector2(posicaoInicial.x, novoY);
    }

    // Função ativada quando algo encosta no Trigger (a caixa de colisão da poção)
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        // Verifica se quem encostou foi o jogador (O GameObject do Damon precisa ter a Tag "Player")
        if (colisao.CompareTag("Player"))
        {
            PlayerMovement scriptJogador = colisao.GetComponent<PlayerMovement>();
            
            if (scriptJogador != null)
            {
                scriptJogador.AdicionarPocao(1); // Dá 1 poção
                Destroy(gameObject); // Destrói a poção do mapa
            }
        }
    }
}