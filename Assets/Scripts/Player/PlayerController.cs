using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera;
    private Vector3 targetPosition;

    private PlayerAbilities playerAbilities;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerAbilities = GetComponent<PlayerAbilities>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAbilityActivation();
    }

    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))  // Left-click for movement
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                targetPosition.y = transform.position.y;  // Prevent height variation
            }
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void HandleAbilityActivation()
    {
        if (Input.GetKeyDown(KeyCode.W))  // Defensive ability (W key)
        {
            playerAbilities.ActivateDefensiveAbility();
        }
        else if (Input.GetKeyDown(KeyCode.Q))  // Wild Card ability (Q key)
        {
            playerAbilities.ActivateWildCardAbility();
        }
        else if (Input.GetKeyDown(KeyCode.E))  // Ultimate ability (E key)
        {
            playerAbilities.ActivateUltimateAbility();
        }
    }
}
