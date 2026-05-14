using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
public class PlayerInventory : MonoBehaviour
{
    [Header("Sistema de Poções")]
    public static int pocoesAtuais = 1; 
    public float valorDeCura = 30f;

    private PlayerEntity atributos;

    void Start()
    {
        atributos = GetComponent<PlayerEntity>();

        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarContadorPocoes(pocoesAtuais);
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
        pocoesAtuais += quantidade;
        
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarContadorPocoes(pocoesAtuais);
        }
    }

    private void UsarPocao()
    {
        if (pocoesAtuais > 0 && atributos.vidaAtual < atributos.vidaMaxima)
        {
            pocoesAtuais--; 

            atributos.Curar(valorDeCura);

            if (UIManager.Instancia != null)
            {
                UIManager.Instancia.AtualizarContadorPocoes(pocoesAtuais);
            }
        }
    }
}