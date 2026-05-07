using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Armas e Efeitos")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Animator vfxAnimator; 

    private Animator anim;
    private PlayerEntity atributos;

    void Start()
    {
        anim = GetComponent<Animator>();
        atributos = GetComponent<PlayerEntity>();
    }

    void Update()
    {
        if (atributos.vidaAtual <= 0) return;

        bool apertouAtaqueCurto = Input.GetKeyDown(KeyCode.Z);
        bool apertouAtaqueLongo = Input.GetKeyDown(KeyCode.X);

        if (apertouAtaqueCurto) anim.SetTrigger("attackShort");
        if (apertouAtaqueLongo) anim.SetTrigger("attackLong");
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            atributos.TomarDano(10f); 
        }
    }

    public void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void DispararEfeitoEstocada()
    {
        if (vfxAnimator != null)
        {
            vfxAnimator.SetTrigger("Attack"); 
        }
    }
}