using UnityEngine;

public class werewollfEntity : Entity
{
    [Header("Drop")]
    public GameObject itemParaDropar;
    public float alturaDropY = 1.5f;
    
    [Header("Persistência")]
    public string idUnicoBoss = "boss_werewolf"; 
    public StatusJogadorSO statusDamon; 
    
    protected override void ConfigurarAtributos()
    {
        vidaMaxima = 200f;
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        if (statusDamon != null && statusDamon.itensColetados.Contains(idUnicoBoss))
        {
            Destroy(gameObject);
        }
    }

    protected override void Morrer()
    {
        base.Morrer();
        
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