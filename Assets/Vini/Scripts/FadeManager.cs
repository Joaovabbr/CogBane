using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }
    
    private Image fadeImage;
    private bool shouldFadeInOnNextScene = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeCanvas();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        { 
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Cena carregada: {scene.name}");
        
        if (shouldFadeInOnNextScene)
        {
            StartCoroutine(FadeIn(1f));
            shouldFadeInOnNextScene = false;
        }
        else
        {
            ClearScreen();
        }
    }
    
    private void CreateFadeCanvas()
    {
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;
        canvasObj.AddComponent<GraphicRaycaster>();
        
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);
        
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
        
        // NOVO: Desativa o bloqueio de clique por padrão na criação
        fadeImage.raycastTarget = false; 
        
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        DontDestroyOnLoad(canvasObj);
        
        Debug.Log("FadeCanvas criado!");
    }
    
    public IEnumerator FadeOut(float duration)
    {
        Debug.Log("FadeOut iniciado");
        
        // NOVO: Ativa o escudo invisível para o jogador não clicar em nada durante a transição
        fadeImage.raycastTarget = true; 
        
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        
        fadeImage.color = Color.black;
        shouldFadeInOnNextScene = true;
        Debug.Log("FadeOut completo");
    }
    
    public IEnumerator FadeIn(float duration)
    {
        Debug.Log("FadeIn iniciado");
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        
        fadeImage.color = new Color(0, 0, 0, 0);
        
        // NOVO: Desativa o escudo invisível após clarear a tela
        fadeImage.raycastTarget = false; 
        
        Debug.Log("FadeIn completo");
    }
    
    public void ClearScreen()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            
            // NOVO: Garante que a tela limpa não vai bloquear cliques
            fadeImage.raycastTarget = false; 
            
            Debug.Log("Tela limpa!");
        }
    }
}