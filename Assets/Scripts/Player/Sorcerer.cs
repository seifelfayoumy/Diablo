// Sorcerer.cs
using UnityEngine;
using UnityEngine.AI;

public class Sorcerer : BasePlayer
{
    // Sorcerer-specific abilities
    public AudioClip fireballSound;
    public AudioClip teleportSound;
    public AudioClip cloneSound;
    public AudioClip infernoSound;

    public GameObject clonePrefab; // Assign the Clone prefab in the Inspector
    public GameObject infernoPrefab; // Assign the Inferno prefab in the Inspector

    public float fireballDamage = 20f;
    public float infernoInitialDamage = 10f;
    public float infernoDamagePerSecond = 2f;
    public float infernoDuration = 5f;

    protected override void Start()
    {
        base.Start();
    }

    // Fireball ability targeting an enemy
    public void Fireball(GameObject enemy)
    {
        animator.SetTrigger("IsFireball");
        audioSource.PlayOneShot(fireballSound);

        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage((int)fireballDamage);
                playerStats.GainXP(enemyScript.GetXP());
            }
        }
    }

    // Teleport ability to a position
    public void Teleport(Vector3 position)
    {
        animator.SetTrigger("Teleport");
        audioSource.PlayOneShot(teleportSound);

        navMeshAgent.Warp(position);
    }

    // Clone ability at a position
    public void Clone(Vector3 position)
    {
        animator.SetTrigger("Clone");
        audioSource.PlayOneShot(cloneSound);

        // Instantiate clone
        GameObject clone = Instantiate(clonePrefab, position, Quaternion.identity);
        CloneBehavior cloneBehavior = clone.GetComponent<CloneBehavior>();
        if (cloneBehavior != null)
        {
            cloneBehavior.Initialize(this);
        }
    }

    // Inferno ability at a position
    public void Inferno(Vector3 position)
    {
        animator.SetTrigger("Inferno");
        audioSource.PlayOneShot(infernoSound);

        // Instantiate Inferno effect
        GameObject inferno = Instantiate(infernoPrefab, position, Quaternion.identity);
        InfernoBehavior infernoBehavior = inferno.GetComponent<InfernoBehavior>();
        if (infernoBehavior != null)
        {
            infernoBehavior.Initialize(this, infernoInitialDamage, infernoDamagePerSecond, infernoDuration);
        }
    }
}
