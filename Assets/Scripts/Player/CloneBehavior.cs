using UnityEngine;
using System.Collections;

public class CloneBehavior : MonoBehaviour
{
    public float duration = 5f;
    public float explosionDamage = 10f;
    public float explosionRadius = 5f;

    private Sorcerer sorcerer;

    public void Initialize(Sorcerer sorcererInstance)
    {
        sorcerer = sorcererInstance;
        StartCoroutine(CloneRoutine());
    }

    private IEnumerator CloneRoutine()
    {

        yield return new WaitForSeconds(duration);

        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        sorcerer.audioManager.PlaySFX(sorcerer.audioManager.explosionSFX);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)explosionDamage);
                    if(enemy.GetHP() <= 0)
                        sorcerer.playerStats.GainXP(enemy.GetXP());
                }
            }
        }
    }
}
