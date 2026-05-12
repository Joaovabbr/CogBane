using UnityEngine;

// Isso cria um botão no menu do Unity para gerar o arquivo facilmente
[CreateAssetMenu(fileName = "NovoStatusDamon", menuName = "Gear Town/Status do Jogador")]
public class StatusJogadorSO : ScriptableObject
{
    [Header("Atributos de Vida")]
    public float vidaMaxima = 100f;
    public float vidaAtual = 100f;

    [Header("Inventário")]
    public int quantidadePocoes = 0;

    // Função para resetar o jogo quando o jogador morrer de vez ou iniciar um Novo Jogo
    public void ResetarParaNovoJogo()
    {
        vidaAtual = vidaMaxima;
        quantidadePocoes = 0;
    }
}