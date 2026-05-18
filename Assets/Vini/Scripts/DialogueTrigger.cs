using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Diálogos")]
    [TextArea(3, 10)]
    public string[] dialogos; 

    [Header("Retratos")]
    public Sprite[] retratos; 

    [Header("Nomes")]
    public string[] nomes; 

    private bool dialogoJaIniciado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogoJaIniciado)
        {
            dialogoJaIniciado = true;
            IniciarDialogo();
            
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                Destroy(collider);
            }
            
            Destroy(this);
        }
    }

    void IniciarDialogo()
    {
        if (DialogueManager.Instancia != null)
        {
            DialogueManager.Instancia.IniciarDialogo(dialogos, retratos, nomes);
        }
    }
}