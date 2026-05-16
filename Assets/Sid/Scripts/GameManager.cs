using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class GameManager : MonoBehaviour
{
    public StatusJogadorSO statusDamon;

    [Header("Áudio da Interface")]
    public AudioSource audioSource;
    public AudioClip somClique;

    void Awake()
    {
        if (statusDamon != null)
        {
            statusDamon.ResetarParaNovoJogo();
        }
    }

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public void IniciarJogo()
    {
        StartCoroutine(IniciarCena("Floresta 1"));
    }

    private IEnumerator IniciarCena(string nomeDaCena)
    {
        if (audioSource != null && somClique != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(somClique);
            
            yield return new WaitForSecondsRealtime(somClique.length);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.2f);
        }

        SceneManager.LoadScene(nomeDaCena);
    }
}