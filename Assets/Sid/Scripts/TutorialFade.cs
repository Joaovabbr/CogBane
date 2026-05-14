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
        // Verifica se quem entrou na área foi o jogador (Damon)
        if (collision.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Esmaecer(1f)); // 1f = 100% visível
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verifica se o jogador saiu da área
        if (collision.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Esmaecer(0f)); // 0f = 100% invisível
        }
    }

    private IEnumerator Esmaecer(float alphaAlvo)
    {
        // Enquanto o alpha atual não chegar no alvo, continua mudando suavemente
        while (!Mathf.Approximately(grupoTutorial.alpha, alphaAlvo))
        {
            grupoTutorial.alpha = Mathf.MoveTowards(grupoTutorial.alpha, alphaAlvo, velocidadeFade * Time.deltaTime);
            yield return null;
        }
    }
}