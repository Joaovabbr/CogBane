using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NovoStatusDamon", menuName = "Gear Town/Status do Jogador")]
public class StatusJogadorSO : ScriptableObject
{
    [Header("Atributos de Vida")]
    public float vidaMaxima = 100f;
    public float vidaAtual = 100f;

    [Header("Inventário")]
    public int quantidadePocoes = 1;
    [HideInInspector] public int maxPocoes = 1;

    [Header("Habilidades Desbloqueadas")]
    public bool garraDesbloqueada = false; 

    [Header("Mundo (Persistência)")]
    public List<string> itensColetados = new List<string>(); 

    public void ResetarParaNovoJogo()
    {
        vidaAtual = vidaMaxima;
        quantidadePocoes = 1;
        maxPocoes = 1;
        garraDesbloqueada = false;
        itensColetados.Clear(); 
    }
}