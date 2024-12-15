using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithBehavior : Enemy
{
    private Animator animator;
    private Transform player; // Reference to the player's transform
    public GameObject minionPrefab; // Reference to the minion prefab
    public float spawnRadius = 15f; // Radius within which minions will be spawned

    private bool isPerformingAction = false; // To prevent overlapping actions
    private bool isTriggered = false; // To start actions only after the player triggers it
    private List<GameObject> activeMinions = new List<GameObject>(); // List to track active minions

    public float actionCooldown = 3f; // Cooldown between actions
    private int countAttack = 0; // Tracks the sequence of actions (0 = Summon, 1 = Divebomb)
    
    protected override void Start()
    {
        // base.Start();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player GameObject

    }

    protected override void Update()
    {
        base.Update();
        // Make Lilith look at the player if it exists
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the y-axis the same to prevent tilting up/down
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust rotation speed as needed
        }
        if (activeMinions.Count > 0)
        {
            isInvincible = true;
            // Debug.Log("Lilith is invincible while minions are active.");
        }else
        {
            isInvincible = false;
        }
    


        // The attack sequence should only proceed if Lilith has been triggered and is not performing an action
        if (isTriggered && !isPerformingAction)
        {
            StartCoroutine(PerformNextAction());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player touches Lilith, start the action cycle
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            if (!isPerformingAction && activeMinions.Count == 0)
            {
                StartCoroutine(PerformNextAction());
            }
        }
    }

    private IEnumerator PerformNextAction()
    {
        isPerformingAction = true;

        if(activeMinions.Count != 0)
        {
            countAttack = 1;
        }

        if (countAttack == 0 && activeMinions.Count == 0)
        {
            // Summon minions
            animator.SetTrigger("Summon");
            yield return new WaitForSeconds(1f); // Wait for the summon animation
            SummonMinions();
            countAttack++;
        }
        else if (countAttack == 1)
        {
            // Perform Divebomb
            animator.SetTrigger("Divebomb");
            DiveBombAttack();
            yield return new WaitForSeconds(1f); // Wait for the divebomb animation
            countAttack = 0; // Reset to summon in the next cycle
        }

        // Wait for cooldown
        yield return new WaitForSeconds(actionCooldown);
        isPerformingAction = false;
    }

    private void SummonMinions()
    {
        int minionCount = 3; // Adjust this to change how many minions to spawn each time
        List<Vector3> usedPositions = new List<Vector3>(); // Track used positions to avoid duplicates

        for (int i = 0; i < minionCount; i++)
        {
            if (minionPrefab != null)
            {
                Vector3 randomPosition = Vector3.zero; // Initialize to avoid the unassigned error
                bool isUniquePosition = false;

                // Keep generating a new position until we find one that hasn't been used
                while (!isUniquePosition)
                {
                    randomPosition = GetRandomPositionAroundLilith(spawnRadius);
                    isUniquePosition = true;

                    // Check if the generated position is already in the list of used positions
                    foreach (Vector3 position in usedPositions)
                    {
                        if (Vector3.Distance(randomPosition, position) < 0.1f) // Use a small tolerance to check for near duplicates
                        {
                            isUniquePosition = false;
                            break;
                        }
                    }
                }

                // Add the new position to the list of used positions
                usedPositions.Add(randomPosition);

                // Instantiate the minion at the unique position
                GameObject minion = Instantiate(minionPrefab, randomPosition, Quaternion.identity);
                activeMinions.Add(minion);

                // Track when the minion is destroyed
                MinionWatcher(minion);
            }
        }
    }

    private Vector3 GetRandomPositionAroundLilith(float radius)
    {
        // Generate a random point within a radius around the Lilith's position
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;
        return randomPosition;
    }

    private void MinionWatcher(GameObject minion)
    {
        StartCoroutine(WatchMinion(minion));
    }

    private IEnumerator WatchMinion(GameObject minion)
    {
        while (minion != null)
        {
            yield return null; // Wait until the minion is destroyed
        }

        if (activeMinions.Contains(minion))
        {
            Debug.Log("Minion destroyed and removed from activeMinions list.");
            activeMinions.Remove(minion);
        }
    }



    public void DiveBombAttack()
    {
        // Check if the player is within a radius of 10 units around Lilith
        if (player != null && Vector3.Distance(transform.position, player.position) <= 10f)
        {
            Debug.Log("Divebomb attack executed on player: " + player.GetComponent<BasePlayer>());
            player.GetComponent<BasePlayer>().TakeDamage(20);
        }
        else
        {
            Debug.Log("Player is not within the range for a divebomb attack.");
        }
    }
}
