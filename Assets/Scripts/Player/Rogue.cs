using UnityEngine;

public class Rogue : BasePlayer
{
    // Rogue-specific abilities
    public AudioClip dashSound;
    public float dashSpeed = 10f;

    protected override void Start()
    {
        base.Start();  // Calls the base Start() method to initialize common properties
    }

    // Override Dash ability
    public  void Attack()
    {
        Debug.Log("Arrow Attack");
        // Special Rogue attack logic goes here
    }

    public void Dash()
    {
        Debug.Log("Dash Activated");
        // Implement dash logic (speed boost for movement)
        navMeshAgent.speed = dashSpeed;
        audioSource.PlayOneShot(dashSound); // Play dash sound
    }
}
