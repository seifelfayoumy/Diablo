
using UnityEngine;

public class Barbarian : BasePlayer
{
    // Ability-specific variables
    public AudioClip shieldSound;
    public AudioClip bashSound;
    public float bashDamage = 15f;

    protected override void Start()
    {
        base.Start();  // Calls the base Start() method to initialize common properties
    }

    protected override void Update()
    {
        base.Update();  // Keeps the movement handling

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Bash(); // Call the Bash method when Space is pressed
        }
    }

    // Override for Barbarian-specific attack (Bash)
    public  void Bash()
    {
        base.animator.SetTrigger("Bash");

        // Add Bash functionality
        Debug.Log("Bash attack");
        audioSource.PlayOneShot(bashSound); // Play bash sound
    }

    // Override Shield (specific for Barbarian)
    public  void Shield()
    {
        audioSource.PlayOneShot(shieldSound);  // Play shield sound
        Debug.Log("Shield Activated");
    }
}
