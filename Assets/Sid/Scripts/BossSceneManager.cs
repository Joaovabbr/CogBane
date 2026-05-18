using UnityEngine;
using UnityEngine.UI;

public class GerenciadorArenaBoss : MonoBehaviour
{
    [Header("Referências Principais")]
    public werewollfEntity bossScript; 
    public werewolfAI bossIA; 
    
    [Header("Interface do Boss")]
    public GameObject canvasBossHUD; 
    public Image barraDeVidaFill; 

    [Header("Eventos Pós-Morte")]
    public GameObject objetoTutorialGarra; 

    [Header("Músicas da Arena")] 
    public AudioClip musicaDoBoss;
    public AudioClip musicaDaFloresta;

    private bool bossJaMorreu = false;
    private bool lutaIniciada = false; 

    void Start()
    {
        if (bossScript == null || bossScript.vidaAtual <= 0)
        {
            AtivarEstadoMorto();
        }
        else
        {
            if (canvasBossHUD != null) canvasBossHUD.SetActive(false);
            if (barraDeVidaFill != null) barraDeVidaFill.gameObject.SetActive(false);
            if (objetoTutorialGarra != null) objetoTutorialGarra.SetActive(false);
        }
    }

    void Update()
    {
        if (bossJaMorreu) return;

        if (bossScript != null)
        {
            if (lutaIniciada && barraDeVidaFill != null)
            {
                barraDeVidaFill.fillAmount = bossScript.vidaAtual / bossScript.vidaMaxima;
            }

            if (bossScript.vidaAtual <= 0)
            {
                AtivarEstadoMorto();
            }
        }
        else
        {
            AtivarEstadoMorto();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!bossJaMorreu && !lutaIniciada && other.CompareTag("Player"))
        {
            lutaIniciada = true;
            
            if (canvasBossHUD != null) canvasBossHUD.SetActive(true);
            if (barraDeVidaFill != null) barraDeVidaFill.gameObject.SetActive(true);
            if (bossIA != null) bossIA.IniciarCombate(); 
            
            if (MusicManager.Instancia != null && musicaDoBoss != null)
            {
                MusicManager.Instancia.TocarMusica(musicaDoBoss);
            }
        }
    }

    private void AtivarEstadoMorto()
    {
        if (bossJaMorreu) return; 

        bossJaMorreu = true;
        lutaIniciada = false;
        
        if (canvasBossHUD != null) canvasBossHUD.SetActive(false); 
        if (barraDeVidaFill != null) barraDeVidaFill.gameObject.SetActive(false);
        if (objetoTutorialGarra != null) objetoTutorialGarra.SetActive(true); 

        if (MusicManager.Instancia != null && musicaDaFloresta != null)
        {
            MusicManager.Instancia.TocarMusica(musicaDaFloresta);
        }
    }
}