using UnityEngine;

public class WolfEntity : Entity
{
    [Header("Sons do Lobo")]
    public AudioSource audioSource;
    public AudioClip somDanoLobo;
    [Range(0f, 1f)]
    public float volumeDano = 1.0f;

    protected override void ConfigurarAtributos()
    {
        vidaMaxima = 30f;
        // vidaAtual é definido automaticamente pelo Awake da Entity
    }

    public override void TomarDano(float dano, string type)
    {
        float vidaAntes = vidaAtual;

        base.TomarDano(dano, type);

        // Toca o som apenas se o dano realmente entrou
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