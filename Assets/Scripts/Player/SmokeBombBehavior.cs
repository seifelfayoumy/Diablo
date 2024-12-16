using UnityEngine;
using System.Collections;

public class SmokeBombBehavior : MonoBehaviour
{
    public float initialDamage = 10f;
    public float damagePerSecond = 2f;
    public float duration = 5f;

    public float radius = 5f;

    private Rogue sorcerer;

    public void Initialize(Rogue sorcererInstance, float initialDmg, float dmgPerSec, float dur)
    {
        sorcerer = sorcererInstance;
        initialDamage = initialDmg;
        damagePerSecond = dmgPerSec;
        duration = dur;
        Debug.Log("Smoke bomb initialized");
        StartCoroutine(SmokeRoutine());
    }

    private IEnumerator SmokeRoutine()
    {
        DealInitialDamage();

        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return new WaitForSeconds(1f);
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
                    enemy.Stun(5);
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
                    sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }
    }
}
