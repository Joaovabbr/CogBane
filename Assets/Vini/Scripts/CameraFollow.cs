using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configurações da Câmera")]
    public Transform alvo; 
    public float suavidade = 5f; 
    public Vector3 ajustePosicao = new Vector3(0f, 2f, -10f); 

    void LateUpdate()
    {
        if (alvo != null)
        {
            Vector3 posicaoDesejada = alvo.position + ajustePosicao;
            transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavidade * Time.deltaTime);
        }
    }
}   