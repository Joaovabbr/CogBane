using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [Header("Atributos Definidos pela Classe Filha")]
    public float vidaMaxima;
    public float vidaAtual;

    [Header("Configurações de Dano")]
    protected float tempoUltimoDano = -100f;
    public bool invencible = false;

    protected Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        ConfigurarAtributos();
        
        if (vidaAtual <= 0) vidaAtual = vidaMaxima; 
    }

    protected abstract void ConfigurarAtributos();

    public virtual void TomarDano(float dano, string type)
    {
        if (invencible) return;

        float tempoInvencivel = type == "enemy" ? 0.2f : 0.5f;
        if (Time.time - tempoUltimoDano < tempoInvencivel) return;

        tempoUltimoDano = Time.time;
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    protected virtual void Morrer()
    {
        if (anim != null) anim.SetTrigger("die");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Collider2D[] todosColisores = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in todosColisores)
        {
            col.enabled = false;
        }
    }
}