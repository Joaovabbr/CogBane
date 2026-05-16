using UnityEngine;

// Mudamos a herança aqui!
public class WolfEntity : EnemyEntity 
{
    [Header("Sons do Lobo")]
    public AudioSource audioSource;
    public AudioClip somDanoLobo;
    [Range(0f, 1f)]
    public float volumeDano = 1.0f;

    protected override void ConfigurarAtributos()
    {
        vidaMaxima = 30f;
    }

    public override void TomarDano(float dano, string type)
    {
        float vidaAntes = vidaAtual;
        base.TomarDano(dano, type);

        if (vidaAtual < vidaAntes)
        {
            if (audioSource != null && somDanoLobo != null)
            {
                audioSource.Stop();
                audioSource.pitch = Random.Range(0.85f, 1.15f);
                audioSource.PlayOneShot(somDanoLobo, volumeDano);
            }
        }
    }
}