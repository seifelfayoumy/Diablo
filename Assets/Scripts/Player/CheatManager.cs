using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviour
{
    private BasePlayer player;
    private PlayerStats playerStats;
    private AbilityManager abilityManager;
    public GameObject pausePanel;
    private bool isPaused = false;
     public GameObject gameOverPanel;
    public AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        player = FindObjectOfType<BasePlayer>();
        playerStats = player.GetComponent<PlayerStats>();
        abilityManager = FindObjectOfType<AbilityManager>();
    }

    void Update()
    {
        if(player.playerHealth.IsDead)
        {
            Time.timeScale = 1f;
            gameOverPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            player.Heal(20);
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Healed 20 HP");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            player.TakeDamage(20);
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Took 20 Damage");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            player.playerHealth.SetInvincibility(!player.playerHealth.isInvincible);
            Debug.Log($"Cheat: Invincibility {(player.playerHealth.isInvincible ? "Enabled" : "Disabled")}");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = (Time.timeScale == 1f) ? 0.5f : 1f;
            Debug.Log($"Cheat: Slow Motion {(Time.timeScale == 0.5f ? "Enabled" : "Disabled")}");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var ability in abilityManager.abilities)
            {
                ability.isOnCooldown = !ability.isOnCooldown;
                ability.cooldownTimer = (ability.isOnCooldown) ? ability.cooldown : 0f;
            }
            Debug.Log("Cheat: Toggle Cooldowns");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (var ability in abilityManager.abilities)
            {
                ability.isUnlocked = true;
            }
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: All Abilities Unlocked");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            playerStats.abilityPoints += 1;
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Gained 1 Ability Point");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            playerStats.GainXP(100);
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Gained 100 XP");
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true); 
        isPaused = true;
        Debug.Log("Game Paused");
        audioManager.StopMusic();
        audioManager.PlayMusic(audioManager.menuMusic);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;
        Debug.Log("Game Resumed");
        audioManager.StopMusic();
        audioManager.PlayMusic(audioManager.mainLevelMusic);
    }
    public void RestartGame()
    {
      GameObject characterInstance = GameObject.FindGameObjectWithTag("Player");
      if(characterInstance != null)
      {
        Destroy(characterInstance);
      }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void GameOver()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over!");
    }
}
