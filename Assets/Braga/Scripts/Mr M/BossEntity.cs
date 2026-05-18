using UnityEngine;

public class BossEntity : Entity
{
    public AudioClip somTomarDano;
    private  AudioSource audioSource;
    protected override void ConfigurarAtributos()
    {
        vidaMaxima = 350f;
    }
    public override void TomarDano(float dano, string type)
    {
        float vidaAntes = vidaAtual;
        base.TomarDano(dano, type);

        if (vidaAtual < vidaAntes)
        {
            TocarSomDeDano();
        }
    }
    public void TocarSomDeDano()
    {
        audioSource = GetComponent<AudioSource>();
        if (somTomarDano != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(somTomarDano);
            
            audioSource.pitch = 1f; 
        }
    }
}
