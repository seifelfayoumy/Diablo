// CloneBehavior.cs
using UnityEngine;
using System.Collections;

public class CloneBehavior : MonoBehaviour
{
    public float duration = 5f; // Duration the clone remains active
    public float explosionDamage = 20f; // Damage dealt upon explosion
    public float explosionRadius = 5f; // Radius of the explosion

    private Sorcerer sorcerer;

    public void Initialize(Sorcerer sorcererInstance)
    {
        sorcerer = sorcererInstance;
        StartCoroutine(CloneRoutine());
    }

    private IEnumerator CloneRoutine()
    {
        // Optional: Play clone spawn animation or effect here

        yield return new WaitForSeconds(duration);

        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Optional: Play explosion animation or effect here

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)explosionDamage);
                    sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }

        Debug.Log("Clone exploded, dealing damage to nearby enemies.");
    }
}
