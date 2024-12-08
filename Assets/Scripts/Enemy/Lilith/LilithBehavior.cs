using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithBehavior : Enemy
{
    private Animator animator;
    private Transform player; // Reference to the player's transform
    public GameObject minionPrefab; // Reference to the minion prefab
    public Transform[] summonPoints; // Points where minions will be summoned

    private bool isSummoning = false; // To prevent multiple summoning at the same time
    private bool isAttacking = false; // To prevent overlapping attacks
    private bool touched = false; // If Lilith started the attacks or not
    private List<GameObject> activeMinions = new List<GameObject>(); // List to track active minions

    public float divebombCooldown = 3f; // Cooldown between Divebomb attacks
    private float lastDivebombTime; // Time of the last Divebomb attack

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player GameObject
        lastDivebombTime = -divebombCooldown; // Initialize to allow an immediate attack
    }

    void Update()
    {
        // Make Lilith look at the player if it exists
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the y-axis the same to prevent tilting up/down
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust rotation speed as needed
        }

        // Check if all minions are defeated
        if (activeMinions.Count > 0)
        {
            activeMinions.RemoveAll(minion => minion == null); // Remove destroyed minions from the list
        }

        // If Lilith has been touched and is idle, start the sequence
        if (touched && !isAttacking && !isSummoning)
        {
            if (activeMinions.Count == 0) // If no minions are active, start summoning
            {
                StartCoroutine(SummonMinions());
            }
            else if (activeMinions.Count == 0 && Time.time >= lastDivebombTime + divebombCooldown) // If all minions are dead, start Divebomb attack
            {
                StartCoroutine(DivebombAttack());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player touches Lilith, enable the attack cycle
        if (other.CompareTag("Player"))
        {
            touched = true;
            Debug.Log("Player touched Lilith.");
        }
    }

    private IEnumerator SummonMinions()
    {
        isSummoning = true;

        // Trigger the summon animation
        animator.SetTrigger("Summon");

        // Wait for the animation to play (adjust time to match your animation)
        yield return new WaitForSeconds(2f);

        // Spawn minions at summon points
        foreach (Transform point in summonPoints)
        {
            GameObject minion = Instantiate(minionPrefab, point.position, Quaternion.identity);
            activeMinions.Add(minion); // Add the spawned minion to the activeMinions list
        }

        isSummoning = false;
    }

    private IEnumerator DivebombAttack()
    {
        isAttacking = true;

        // Trigger the Divebomb animation
        animator.SetTrigger("Divebomb");

        // Wait for the animation to play (adjust time to match your animation)
        yield return new WaitForSeconds(1.5f);

        // Simulate the Divebomb attack (e.g., applying damage or an effect to the player)
        Debug.Log("Lilith performs a Divebomb attack!");

        // Record the time of this attack
        lastDivebombTime = Time.time;

        isAttacking = false;
    }
}
