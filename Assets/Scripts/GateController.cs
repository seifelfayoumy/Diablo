using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    public string bossSceneName = "BossLevel";
    public int requiredFragments = 3;
    public AudioClip gateOpenSound;
    private bool gateOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                if (playerStats.runeFragments >= requiredFragments)
                {
                    if (!gateOpened)
                    {
                        OpenGate();
                    }
                    LoadBossScene();
                }
                else
                {
                    Debug.Log("You need " + requiredFragments + " Rune Fragments to open this gate!");
                }
            }
        }
    }

    void OpenGate()
    {
        if (gateOpenSound != null)
        {
            AudioSource.PlayClipAtPoint(gateOpenSound, transform.position);
        }

        gateOpened = true;
    }

    void LoadBossScene()
    {
        SceneManager.LoadScene(bossSceneName);
    }
}
