using System;
using UnityEngine;

public class BossAttackFlamethrower : MonoBehaviour
{
    public BossAI boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.flamethrowerRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.flamethrowerRange = false;
        }
    }
}
