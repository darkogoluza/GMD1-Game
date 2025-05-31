using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI teamWonText;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button button;

    private void Awake()
    {
        menu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
       Application.Quit(); 
    }

    public void OpenEndGameMenu(int teamWon)
    {
        teamWonText.text = $"Team {teamWon} Won!";
        menu.SetActive(true);
        Time.timeScale = 0f;
        eventSystem.SetSelectedGameObject(button.gameObject);
    }
}
