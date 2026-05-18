using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class UpgradeGarra : MonoBehaviour
{
    [Header("Configurações")]
    public float alturaFlutuacao = 0.3f;
    public float velocidadeFlutuacao = 2f;
    
    [Header("Áudio")] 
    public AudioClip somColeta;
    
    [Header("Persistência")]
    public string idUnicoUpgrade = "upgrade_garra_boss";
    public StatusJogadorSO statusDamon; 
    
    private Vector3 posicaoInicial;
    
    void Start()
    {
        if (statusDamon != null && statusDamon.itensColetados.Contains(idUnicoUpgrade))
        {
            Destroy(gameObject);
            return;
        }
        
        posicaoInicial = transform.position;
        GetComponent<Collider2D>().isTrigger = true;
    }
    
    void Update()
    {
        float novaY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuacao) * alturaFlutuacao;
        transform.position = new Vector3(posicaoInicial.x, novaY, posicaoInicial.z);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            
            if (playerInventory != null && playerInventory.statusDamon != null)
            {
                playerInventory.statusDamon.garraDesbloqueada = true;
                
                if (!playerInventory.statusDamon.itensColetados.Contains(idUnicoUpgrade))
                {
                    playerInventory.statusDamon.itensColetados.Add(idUnicoUpgrade);
                }
                
                if (somColeta != null)
                {
                    AudioSource.PlayClipAtPoint(somColeta, transform.position);
                }
                Debug.Log(" Garra desbloqueada! Pressione C ou L para usar.");
                
                Destroy(gameObject);
            }
        }
    }
}