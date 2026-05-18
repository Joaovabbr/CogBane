using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [Header("Música Deste Local")]
    public AudioClip musicaDaCena;

    void Start()
    {
        if (MusicManager.Instancia != null)
        {
            MusicManager.Instancia.TocarMusica(musicaDaCena);
        }
    }
}