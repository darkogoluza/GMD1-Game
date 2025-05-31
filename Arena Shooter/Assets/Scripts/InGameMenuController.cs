using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject inGameMenuPanel;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button button;

    private void Awake()
    {
        inGameMenuPanel.SetActive(false);
    }

    private void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.leftTrigger.wasPressedThisFrame)
            {
                PauseGame();
                break;
            }
        }
    }

    private void PauseGame()
    {
        if (inGameMenuPanel.activeSelf)
        {
            inGameMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            inGameMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            eventSystem.SetSelectedGameObject(button.gameObject);
        }
    }

    public void ResumeGame()
    {
        inGameMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
