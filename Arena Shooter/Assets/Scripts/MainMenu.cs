using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuGO;
    [SerializeField] [NotNull] private GameObject SetUpPlayStep1GO;
    [SerializeField] [NotNull] private GameObject SetUpPlayStep2GO;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button mainMenuFirstButton;
    [SerializeField] private Button stepTwoFirstButton;
    [SerializeField] private Button stepThreeFirstButton;

    private void Awake()
    {
        MainMenuGO.SetActive(true);
        SetUpPlayStep1GO.SetActive(false);
        SetUpPlayStep2GO.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Start()
    {
        AudioManager.Instance.Play("MainMenuBackgroundMusic");
        eventSystem.firstSelectedGameObject = mainMenuFirstButton.gameObject;
    }

    public void GoToStep2()
    {
        MainMenuGO.SetActive(false);
        SetUpPlayStep1GO.SetActive(true);
        SetUpPlayStep2GO.SetActive(false);
        eventSystem.SetSelectedGameObject(stepTwoFirstButton.gameObject);
    }

    public void GoToStep3(int numberOfTeams)
    {
        GlobalSettings.NumberOfTeams = numberOfTeams;
        
        MainMenuGO.SetActive(false);
        SetUpPlayStep1GO.SetActive(false);
        SetUpPlayStep2GO.SetActive(true);
        eventSystem.SetSelectedGameObject(stepThreeFirstButton.gameObject);
    }

    public void GoToMainMenu()
    {
        MainMenuGO.SetActive(true);
        SetUpPlayStep1GO.SetActive(false);
        SetUpPlayStep2GO.SetActive(false);
        eventSystem.SetSelectedGameObject(mainMenuFirstButton.gameObject);
    }

    public void Play(int playersPerTeam)
    {
        GlobalSettings.PlayersPerTeam = playersPerTeam;
        
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
