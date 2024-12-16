using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InfernoBehavior : MonoBehaviour
{
    public float initialDamage = 10f;
    public float damagePerSecond = 2f;
    public float duration = 5f;

    public float radius = 3f;

    private Sorcerer sorcerer;

    public void Initialize(Sorcerer sorcererInstance, float initialDmg, float dmgPerSec, float dur)
    {
        sorcerer = sorcererInstance;
        initialDamage = initialDmg;
        damagePerSecond = dmgPerSec;
        duration = dur;
        StartCoroutine(InfernoRoutine());
    }

    private IEnumerator InfernoRoutine()
    {
        DealInitialDamage();

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return new WaitForSeconds(1f);
            DealDamageOverTime();
            elapsed += 1f;
        }
        Destroy(gameObject);
    }

    private void DealInitialDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)initialDamage);
                    if(enemy.GetHP() <= 0)
                        sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }
    }

    private void DealDamageOverTime()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)damagePerSecond);
                }
            }
        }

        HashSet<Collider> uniqueColliders = new HashSet<Collider>(hitColliders);

        foreach (var hit in uniqueColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if(enemy.GetHP() <= 0)
                        sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }
    }
}
