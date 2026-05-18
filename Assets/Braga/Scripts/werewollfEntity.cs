using UnityEngine;

// Lembre-se: se vocês estiverem usando a arquitetura nova, ele deve herdar de EnemyEntity em vez de Entity!
public class werewollfEntity : Entity 
{
    [Header("Drop")]
    public GameObject itemParaDropar;
    public float alturaDropY = 1.5f;
    
    [Header("Persistência")]
    public string idUnicoBoss = "boss_werewolf"; 
    public StatusJogadorSO statusDamon; 

    [Header("Áudio de Dano")]
    public AudioSource audioSource;
    public AudioClip somDano;
    [Range(0f, 1f)]
    public float volumeDano = 1.0f;
    
    protected override void ConfigurarAtributos()
    {
        vidaMaxima = 200f;
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        // Verifica se o boss já foi morto em uma visita anterior. Se sim, ele se auto-destrói.
        if (statusDamon != null && statusDamon.itensColetados.Contains(idUnicoBoss))
        {
            Destroy(gameObject);
        }
    }

    public override void TomarDano(float dano, string type)
    {
        float vidaAntes = vidaAtual;

        // Chama a lógica original de tomar dano do Entity
        base.TomarDano(dano, type);

        // Toca o som apenas se a vida realmente diminuiu (dano não foi bloqueado por invencibilidade)
        if (vidaAtual < vidaAntes)
        {
            if (audioSource != null && somDano != null)
            {
                // Varia o pitch levemente para o som não ficar repetitivo
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(somDano, volumeDano);
            }
        }
    }

    protected override void Morrer()
    {
        base.Morrer();
        
        // Salva a morte do boss no arquivo do jogador para ele não voltar a nascer
        if (statusDamon != null && !statusDamon.itensColetados.Contains(idUnicoBoss))
        {
            statusDamon.itensColetados.Add(idUnicoBoss);
        }
        
        DroparItem();
    }

    void DroparItem()
    {
        if (itemParaDropar != null)
        {
            Vector3 posicaoDrop = transform.position + new Vector3(0, alturaDropY, 0);
            Instantiate(itemParaDropar, posicaoDrop, Quaternion.identity);
        }
    }
}