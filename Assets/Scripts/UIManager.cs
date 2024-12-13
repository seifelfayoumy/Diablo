using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button barbarianButton;
    public Button sorcererButton;
    public Button rogueButton;
    public Canvas choiceCanvas;

    [Header("Prefabs")]
    public GameObject barbarianPrefab;
    public GameObject sorcererPrefab;
    public GameObject roguePrefab;
        public GameObject HUDManager;
            public GameObject GenericCanvas;

    [Header("Other References")]
    public Camera mainCamera;

    private void Start()
    {
        // Freeze the game at the start
        Time.timeScale = 0f;

        // Assign button listeners
        barbarianButton.onClick.AddListener(() => SelectCharacter(barbarianPrefab));
        sorcererButton.onClick.AddListener(() => SelectCharacter(sorcererPrefab));
        rogueButton.onClick.AddListener(() => SelectCharacter(roguePrefab));
    }

    private void SelectCharacter(GameObject characterPrefab)
    {
        // Spawn the selected character
        GameObject characterInstance = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);

        // Attach the camera follow script
        CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
        }
        cameraFollow.target = characterInstance.transform;

        // Disable the choice canvas
        choiceCanvas.gameObject.SetActive(false);
        HUDManager.SetActive(true);
        GenericCanvas.SetActive(true);

        // Unfreeze the game
        Time.timeScale = 1f;
    }
}


