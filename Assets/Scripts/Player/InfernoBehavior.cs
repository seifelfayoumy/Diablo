// InfernoBehavior.cs
using UnityEngine;
using System.Collections;

public class InfernoBehavior : MonoBehaviour
{
    public float initialDamage = 10f; // Initial damage when Inferno is cast
    public float damagePerSecond = 2f; // Damage per second while enemies are within the Inferno
    public float duration = 5f; // Duration of the Inferno

    public float radius = 5f; // Radius of the Inferno effect

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
        // Deal initial damage to enemies within the radius
        DealInitialDamage();

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return new WaitForSeconds(1f);
            DealDamageOverTime();
            elapsed += 1f;
        }

        // Optional: Play inferno end animation or effect here

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
                    sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }

        Debug.Log("Inferno cast, dealing initial damage to enemies.");
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
                    sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }

        Debug.Log("Inferno deals damage over time to enemies within the radius.");
    }

    // Optional: Visual representation (e.g., particle effects) can be added here
}
