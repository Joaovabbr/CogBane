using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
public class PlayerInventory : MonoBehaviour
{
    [Header("Referências")]
    public StatusJogadorSO statusDamon;

    [Header("Configurações de Cura")]
    public float valorDeCura = 30f;

    private PlayerEntity atributos;
    private bool dentroAreaFullHeal = false;

    void Start()
    {
        atributos = GetComponent<PlayerEntity>();

        if (statusDamon.quantidadePocoes > statusDamon.maxPocoes)
        {
            statusDamon.maxPocoes = statusDamon.quantidadePocoes;
        }

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

        if (Input.GetKeyDown(KeyCode.R) && dentroAreaFullHeal)
        {
            UsarFullHeal();
        }
    }

    public void AdicionarPocao(int quantidade)
    {
        statusDamon.quantidadePocoes += quantidade;
        
        if (statusDamon.quantidadePocoes > statusDamon.maxPocoes)
        {
            statusDamon.maxPocoes = statusDamon.quantidadePocoes;
        }
        
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarContadorPocoes(statusDamon.quantidadePocoes);
        }
    }

    public void UsarPocao()
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

    public void UsarFullHeal()
    {
        float curaTotal = statusDamon.vidaMaxima - statusDamon.vidaAtual;
        if (curaTotal > 0)
        {
            atributos.Curar(curaTotal);
        }

        statusDamon.quantidadePocoes = statusDamon.maxPocoes;

        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarContadorPocoes(statusDamon.quantidadePocoes);
        }

        Debug.Log($"Full Heal usado! Vida restaurada. Poções: {statusDamon.quantidadePocoes}/{statusDamon.maxPocoes}");
    }

    public void EntrarAreaFullHeal()
    {
        dentroAreaFullHeal = true;
    }

    public void SairAreaFullHeal()
    {
        dentroAreaFullHeal = false;
    }
}