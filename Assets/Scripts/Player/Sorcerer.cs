using UnityEngine;

public class Sorcerer : BasePlayer
{
    // Sorcerer-specific abilities
    public AudioClip fireballSound;
    public float fireballDamage = 20f;

    protected override void Start()
    {
        base.Start();  // Calls the base Start() method to initialize common properties
    }

    // Sorcerer attack overrides (e.g., Fireball)
    public  void Attack()
    {
        Debug.Log("Fireball Attack");
        audioSource.PlayOneShot(fireballSound); // Play fireball sound
    }
}
