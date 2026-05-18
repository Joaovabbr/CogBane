using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryIntro : MonoBehaviour
{
    [Tooltip("Digite o nome exato da cena que deve carregar (ex: Fase1)")]
    public string nextSceneName; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}