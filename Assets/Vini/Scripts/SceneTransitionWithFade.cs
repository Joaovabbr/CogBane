using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionWithFade : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string targetSceneName; 
    [SerializeField] private Vector3 spawnPosition; 
    
    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 1f;
    
    private bool isTransitioning = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionToScene());
        }
    }
    
    private IEnumerator TransitionToScene()
    {
        PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
        PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);
        PlayerPrefs.SetFloat("SpawnZ", spawnPosition.z);
        PlayerPrefs.Save();
        
        yield return StartCoroutine(FadeManager.Instance.FadeOut(fadeDuration));
        
        SceneManager.LoadScene(targetSceneName);
    }
}