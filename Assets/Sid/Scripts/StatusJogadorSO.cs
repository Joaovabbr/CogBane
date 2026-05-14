using UnityEngine;

[CreateAssetMenu(fileName = "NovoStatusDamon", menuName = "Gear Town/Status do Jogador")]
public class StatusJogadorSO : ScriptableObject
{
    [Header("Atributos de Vida")]
    public float vidaMaxima = 100f;
    public float vidaAtual = 100f;

    [Header("Inventário")]
    public int quantidadePocoes = 1;

    public void ResetarParaNovoJogo()
    {
        vidaAtual = vidaMaxima;
        quantidadePocoes = 1;
    }
}