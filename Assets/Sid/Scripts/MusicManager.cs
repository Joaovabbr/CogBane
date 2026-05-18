using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instancia;

    [Header("Componente de Áudio")]
    public AudioSource audioSource;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TocarMusica(AudioClip novaMusica)
    {
        if (audioSource == null) return;

        if (novaMusica == null)
        {
            audioSource.Stop();
            audioSource.clip = null;
            return;
        }

        if (audioSource.clip == novaMusica && audioSource.isPlaying) return;

        audioSource.clip = novaMusica;
        audioSource.Play();
    }
}