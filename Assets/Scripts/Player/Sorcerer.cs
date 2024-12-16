using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Sorcerer : BasePlayer
{
    public AudioClip fireballSound;
    public AudioClip teleportSound;
    public AudioClip cloneSound;
    public AudioClip infernoSound;

    public GameObject clonePrefab;
    public GameObject infernoPrefab;

    public float fireballDamage = 5f;
    public float infernoInitialDamage = 10f;
    public float infernoDamagePerSecond = 2f;
    public float infernoDuration = 5f;

    public GameObject fireballPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public void Fireball(GameObject enemy)
    {
        animator.SetTrigger("IsFireball");
        
        StartCoroutine(FireballSpawn(enemy));
    }

    IEnumerator FireballSpawn(GameObject enemy)
    {
        yield return new WaitForSeconds(2f);

        Vector3 lookAt = new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
        transform.LookAt(lookAt);
        
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        spawnPosition.y = 1f;

        audioManager.PlaySFX(audioManager.fireballSFX);
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, transform.rotation);

        Fireball fireballScript = fireball.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            fireballScript.Initialize(10, 5, enemy, playerStats);
        }

        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage((int)fireballDamage);
                if(enemyScript.GetHP() <= 0)
                    playerStats.GainXP(enemyScript.GetXP());
            }
        }
    }

    public void Teleport(Vector3 position)
    {
        navMeshAgent.Warp(position);
    }

    public void Clone(Vector3 position)
    {
        audioManager.PlaySFX(audioManager.cloneSFX);

        GameObject clone = Instantiate(clonePrefab, position, Quaternion.identity);
        CloneBehavior cloneBehavior = clone.GetComponent<CloneBehavior>();
        if (cloneBehavior != null)
        {
            cloneBehavior.Initialize(this);
        }
    }

    public void Inferno(Vector3 position)
    {
        animator.SetTrigger("IsInferno");
        audioManager.PlaySFX(audioManager.infernoSFX);

        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);

        GameObject inferno = Instantiate(infernoPrefab, position, Quaternion.identity);
        InfernoBehavior infernoBehavior = inferno.GetComponent<InfernoBehavior>();
        if (infernoBehavior != null)
        {
            infernoBehavior.Initialize(this, infernoInitialDamage, infernoDamagePerSecond, infernoDuration);
        }
    }
}
