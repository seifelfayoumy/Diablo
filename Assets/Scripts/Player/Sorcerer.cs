// Sorcerer.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Sorcerer : BasePlayer
{
    // Sorcerer-specific abilities
    public AudioClip fireballSound;
    public AudioClip teleportSound;
    public AudioClip cloneSound;
    public AudioClip infernoSound;

    public GameObject clonePrefab; // Assign the Clone prefab in the Inspector
    public GameObject infernoPrefab; // Assign the Inferno prefab in the Inspector

    public float fireballDamage = 5f;
    public float infernoInitialDamage = 10f;
    public float infernoDamagePerSecond = 2f;
    public float infernoDuration = 5f;

    public GameObject fireballPrefab; // Assign the enemy GameObject in the Inspector

    protected override void Start()
    {
        base.Start();
    }

    // public void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.J))
    //     {
    //         Inferno(transform.position);
    //         Debug.Log("Inferno");
    //     }
    // }

    // Fireball ability targeting an enemy
    public void Fireball(GameObject enemy)
    {
        animator.SetTrigger("IsFireball");
        audioSource.PlayOneShot(fireballSound);
        
        StartCoroutine(FireballSpawn(enemy));
    }

    IEnumerator FireballSpawn(GameObject enemy)
    {
        // Adjust rotation
        Vector3 position = enemy.transform.position;
        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);

        yield return new WaitForSeconds(2f);
        
        // Spawn fireball in front of the player with Y = 1
        Vector3 spawnPosition = transform.position + transform.forward * 2f; // 2f is the distance in front of the player
        spawnPosition.y = 1f;  // Set Y to 1

        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, transform.rotation);

        Fireball fireballScript = fireball.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            fireballScript.Initialize(10, 5, enemy, playerStats);
            Debug.Log("Fireball");
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

    // Teleport ability to a position
    public void Teleport(Vector3 position)
    {
        audioSource.PlayOneShot(teleportSound);

        navMeshAgent.Warp(position);
    }

    // Clone ability at a position
    public void Clone(Vector3 position)
    {
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
        animator.SetTrigger("IsInferno");
        audioSource.PlayOneShot(infernoSound);

        // Adjust rotation
        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);

        // Instantiate Inferno effect
        GameObject inferno = Instantiate(infernoPrefab, position, Quaternion.identity);
        InfernoBehavior infernoBehavior = inferno.GetComponent<InfernoBehavior>();
        if (infernoBehavior != null)
        {
            infernoBehavior.Initialize(this, infernoInitialDamage, infernoDamagePerSecond, infernoDuration);
        }
    }
}
