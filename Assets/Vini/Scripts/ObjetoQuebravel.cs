using UnityEngine;

public class ObjetoQuebravel : MonoBehaviour
{
    [Header("Efeitos (opcional)")]
    public GameObject efeitoQuebradura; // Partículas ao quebrar
    public AudioClip somQuebrar; // Som ao quebrar
    
    public void Quebrar()
    {
        if (efeitoQuebradura != null)
        {
            Instantiate(efeitoQuebradura, transform.position, Quaternion.identity);
        }
        
        if (somQuebrar != null)
        {
            AudioSource.PlayClipAtPoint(somQuebrar, transform.position);
        }
        
        Destroy(gameObject);
    }
}