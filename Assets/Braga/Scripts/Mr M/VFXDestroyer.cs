using UnityEngine;

public class VFXDestroyer : MonoBehaviour
{
    public float tempoDeVida = 1f; 

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }
}
