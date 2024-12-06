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

    protected override void Update()
    {
        base.Update();  // Keeps the movement handling

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SmokeBomb(); // Call the Bash method when Space is pressed
        }
    }

    public void Arrow()
    {
        base.animator.SetTrigger("IsArrow");
    }

    public  void SmokeBomb()
    {
        base.animator.SetTrigger("IsSmoke");
        Debug.Log("Arrow Attack");
        // Special Rogue attack logic goes here
    }

    public void Dash()
    {
        base.animator.SetBool("IsDash", true);
        Debug.Log("Dash Activated");
        // Implement dash logic (speed boost for movement)
        navMeshAgent.speed = dashSpeed;
        audioSource.PlayOneShot(dashSound); // Play dash sound
    }

    public void ShoweOfArrows()
    {
        base.animator.SetTrigger("IsShower");
    }
}
