using UnityEngine;
using System.Collections.Generic; // Necessário para usar Listas

[CreateAssetMenu(fileName = "NovoStatusDamon", menuName = "Gear Town/Status do Jogador")]
public class StatusJogadorSO : ScriptableObject
{
    [Header("Atributos de Vida")]
    public float vidaMaxima = 100f;
    public float vidaAtual = 100f;

    [Header("Inventário")]
    public int quantidadePocoes = 1;

    [Header("Mundo (Persistência)")]
    // Lista que guarda o RG de tudo que já foi pego
    public List<string> itensColetados = new List<string>(); 

    public void ResetarParaNovoJogo()
    {
        vidaAtual = vidaMaxima;
        quantidadePocoes = 1;
        itensColetados.Clear(); 
    }
}