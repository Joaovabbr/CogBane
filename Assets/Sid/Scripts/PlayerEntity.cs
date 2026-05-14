using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerEntity : Entity
{
    [Header("Dados Persistentes")]
    public StatusJogadorSO statusDamon; 

    [Header("Configurações de Movimento do Damon")]
    public float velocidadeAndar;
    public float velocidadeCorrer;
    public float tempoParaCorrer;
    public float forcaPulo;
    public float tempoParaEntrarIdle;

    [Header("Efeitos Visuais Exclusivos")]
    public GameObject damagePopupPrefab;
    public Color corDano = new Color(0.7f, 0f, 0f);
    public Color corCura = new Color(0.18f, 0.8f, 0.44f);

    protected override void ConfigurarAtributos()
    {
        // Puxa a vida salva do arquivo permanente (SO) logo ao carregar a cena
        if (statusDamon != null)
        {
            vidaMaxima = statusDamon.vidaMaxima;
            vidaAtual = statusDamon.vidaAtual;
        }

        velocidadeAndar = 5f;
        velocidadeCorrer = 9f;
        tempoParaCorrer = 0.5f;
        forcaPulo = 14f;
        tempoParaEntrarIdle = 3.0f;
    }

    private void Start()
    {
        // Atualiza a barra de vida no HUD assim que a cena inicia
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarBarraDeVida(vidaAtual, vidaMaxima);
        }
    }

    public override void TomarDano(float dano, string type)
    {
        // Chama a lógica de dano da classe pai (Entity)
        base.TomarDano(dano, "player"); 

        // Atualiza a vida no arquivo permanente na mesma hora para não perder o progresso
        if (statusDamon != null)
        {
            statusDamon.vidaAtual = this.vidaAtual;
        }

        MostrarNumeroPopup(dano.ToString(), corDano);

        // Atualiza a barra de vida no HUD
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarBarraDeVida(vidaAtual, vidaMaxima);
        }
    }

    // A cura é exclusiva do jogador, então fica direto aqui
    public void Curar(float quantidade)
    {
        vidaAtual += quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        // Atualiza a vida curada no arquivo permanente
        if (statusDamon != null)
        {
            statusDamon.vidaAtual = this.vidaAtual;
        }

        MostrarNumeroPopup("+" + quantidade.ToString(), corCura);

        // Atualiza a barra de vida no HUD
        if (UIManager.Instancia != null)
        {
            UIManager.Instancia.AtualizarBarraDeVida(vidaAtual, vidaMaxima);
        }
    }

    private void MostrarNumeroPopup(string texto, Color cor)
    {
        if (damagePopupPrefab != null)
        {
            Vector3 posicaoSpawn = transform.position + new Vector3(0, 1.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, posicaoSpawn, Quaternion.identity);
            
            DamagePopup scriptPopup = popup.GetComponent<DamagePopup>();
            if (scriptPopup != null) scriptPopup.Setup(texto, cor);
        }
    }

    protected override void Morrer()
    {
        // Chama a animação ou lógica base de morte
        base.Morrer(); 
        
        // Desativa os controles do jogador
        PlayerMovement movimento = GetComponent<PlayerMovement>();
        if (movimento != null) movimento.enabled = false; 
        
        StartCoroutine(EsperaCarregarGameOver());
    }

    private IEnumerator EsperaCarregarGameOver()
    {
        // Aguarda 3 segundos em tempo real para o jogador absorver a morte
        yield return new WaitForSecondsRealtime(3f);
    
        // Carrega a cena onde a máquina de escrever vai brilhar
        SceneManager.LoadScene("GameOver");
    }
}