using System.Collections;
using UnityEngine;

public class TutorialFade : MonoBehaviour
{
    [Header("Qual grupo UI vai aparecer?")]
    public CanvasGroup grupoTutorial;
    
    [Header("Velocidade do esmaecimento")]
    public float velocidadeFade = 2f;

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Esmaecer(1f)); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
   
        if (collision.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Esmaecer(0f)); 
        }
    }

    private IEnumerator Esmaecer(float alphaAlvo)
    {
        while (!Mathf.Approximately(grupoTutorial.alpha, alphaAlvo))
        {
            grupoTutorial.alpha = Mathf.MoveTowards(grupoTutorial.alpha, alphaAlvo, velocidadeFade * Time.deltaTime);
            yield return null;
        }
    }
}