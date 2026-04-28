using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nome da cena de destino
    [SerializeField] private float transitionDelay = 0.5f; // Delay opcional
    
    private void OnTriggerEnter2D(Collider2D other) // Ou OnTriggerEnter para 3D
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadSceneWithDelay());
        }
    }
    
    private System.Collections.IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneName);
    }
}