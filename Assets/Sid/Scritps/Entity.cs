using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Atributos Definidos pela Classe Filha")]
    public float vidaMaxima;
    public float vidaAtual;

    protected Animator anim;

    protected virtual void Awake()
    {
        ConfigurarAtributos(); 
        
        vidaAtual = vidaMaxima;
        anim = GetComponent<Animator>();
    }

    protected abstract void ConfigurarAtributos();

    public virtual void TomarDano(float dano)
    {
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
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        this.enabled = false;
    }
}