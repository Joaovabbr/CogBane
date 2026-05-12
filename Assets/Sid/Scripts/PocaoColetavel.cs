using UnityEngine;

public class PocaoColetavel : MonoBehaviour
{
    [Header("Animação de Flutuar")]
    public float velocidadeFlutuar = 2f;
    public float alturaFlutuar = 0.2f;
    private Vector2 posicaoInicial;

    void Start()
    {
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
                Destroy(gameObject); 
            }
        }
    }
}