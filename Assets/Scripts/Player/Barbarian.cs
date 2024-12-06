
using UnityEngine;

public class Barbarian : BasePlayer
{
    // Ability-specific variables
    public AudioClip shieldSound;
    public AudioClip bashSound;
    public float bashDamage = 15f;
    public GameObject shieldObject;

    protected override void Start()
    {
        base.Start();  // Calls the base Start() method to initialize common properties
    }

    protected override void Update()
    {
        base.Update();  // Keeps the movement handling

        if(Input.GetKeyDown(KeyCode.Space))
        {
            IronMaelstorm(); // Call the Bash method when Space is pressed
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Bash(); // Call the Bash method when B is pressed
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shield(); // Call the Shield method when S is pressed
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            charge(); // Call the charge method when C is pressed
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

    public void IronMaelstorm()
    {
        base.animator.SetTrigger("IronMaelstorm");
        Debug.Log("Iron Maelstorm attack");
    }

    // Override Shield (specific for Barbarian)
    public  void Shield()
    {
        base.animator.SetTrigger("Shield");

        // Add Shield functionality
        //spawn shield object
        GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
        shield.transform.SetParent(transform);  // Attach shield to player
        Destroy(shield, 3f);
        audioSource.PlayOneShot(shieldSound);  // Play shield sound
        Debug.Log("Shield Activated");
    }

    public void charge()
    {
        base.animator.SetTrigger("Charge");
        Debug.Log("Charge Activated");
    }

}
