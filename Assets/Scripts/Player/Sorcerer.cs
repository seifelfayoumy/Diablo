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

    protected override void Update()
    {
        base.Update();  // Keeps the movement handling

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Inferno();
        }
    }

    // Sorcerer attack overrides (e.g., Fireball)
    public  void Fireball()
    {
        base.animator.SetTrigger("IsFireball");
        Debug.Log("Fireball Attack");
        audioSource.PlayOneShot(fireballSound); // Play fireball sound
    }

    public void Teleport()
    {

    }

    public void Clone()
    {

    }

    public void Inferno()
    {
        base.animator.SetTrigger("IsInferno");
    }
}
