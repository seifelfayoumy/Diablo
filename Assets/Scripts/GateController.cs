using UnityEngine;
using UnityEngine.SceneManagement; // To load the boss scene

public class GateController : MonoBehaviour
{
    public string bossSceneName = "BossLevel"; // The name of your boss scene
    public int requiredFragments = 3;
    public AudioClip gateOpenSound; // Optional: sound when the gate opens
    private bool gateOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Check Rune Fragment count
                if (playerStats.runeFragments >= requiredFragments)
                {
                    // Player meets the requirement, "open" the gate
                    if (!gateOpened)
                    {
                        OpenGate();
                    }

                    // Proceed to load the boss scene
                    LoadBossScene();
                }
                else
                {
                    // Player does not have enough Rune Fragments
                    Debug.Log("You need " + requiredFragments + " Rune Fragments to open this gate!");
                    // Optionally, you can play a locked sound or show a UI message here.
                }
            }
        }
    }

    void OpenGate()
    {
        // If you have an animation for the gate opening, trigger it here.
        // Example:
        // Animator animator = GetComponent<Animator>();
        // animator.SetTrigger("OpenGate");

        // Play gate open sound if assigned
        if (gateOpenSound != null)
        {
            AudioSource.PlayClipAtPoint(gateOpenSound, transform.position);
        }

        gateOpened = true;
    }

    void LoadBossScene()
    {
        // Load the boss scene
        SceneManager.LoadScene(bossSceneName);
    }
}
