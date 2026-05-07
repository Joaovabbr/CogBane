using UnityEngine;

public class WolfEntity : Entity
{
    [Header("Configurações de Movimento")]
    public float velocidadeAndar;
    public float velocidadeCorrer;
    public float tempoParaCorrer;
    public float forcaPulo;
    public float tempoParaEntrarIdle;
    

    protected override void ConfigurarAtributos()
    {
        vidaMaxima = 30f;
        velocidadeAndar = 5f;
        velocidadeCorrer = 9f;
        tempoParaCorrer = 0.5f;
        forcaPulo = 14f;
        tempoParaEntrarIdle = 3.0f;
    }

    private void Start()
    {
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarBarraDeVida(vidaAtual, vidaMaxima);
        }
    }

    public override void TomarDano(float dano)
    {
        base.TomarDano(dano); 
        

        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarBarraDeVida(vidaAtual, vidaMaxima);
        }
    }
    

    protected override void Morrer()
    {
        base.Morrer(); 
        
        PlayerMovement movimento = GetComponent<PlayerMovement>();
        if (movimento != null) movimento.enabled = false; 
    }
}