using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
public class PlayerInventory : MonoBehaviour
{
    [Header("Referências")]
    public StatusJogadorSO statusDamon;

    [Header("Configurações de Cura")]
    public float valorDeCura = 30f;

    private PlayerEntity atributos;

    void Start()
    {
        atributos = GetComponent<PlayerEntity>();

        if (UIManager.Instancia != null && statusDamon != null)
        {
            UIManager.Instancia.AtualizarContadorPocoes(statusDamon.quantidadePocoes);
        }
    }

    void Update()
    {
        if (atributos.vidaAtual <= 0) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            UsarPocao();
        }
    }

    public void AdicionarPocao(int quantidade)
    {
        statusDamon.quantidadePocoes += quantidade;
        
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarContadorPocoes(statusDamon.quantidadePocoes);
        }
    }

    private void UsarPocao()
    {
        if (statusDamon.quantidadePocoes > 0 && statusDamon.vidaAtual < statusDamon.vidaMaxima)
        {
            statusDamon.quantidadePocoes--; 

            atributos.Curar(valorDeCura);

            if (UIManager.Instancia != null)
            {
                UIManager.Instancia.AtualizarContadorPocoes(statusDamon.quantidadePocoes);
            }
        }
    }
}