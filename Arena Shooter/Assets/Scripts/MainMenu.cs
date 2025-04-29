using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuGO;
    [SerializeField] [NotNull] private GameObject SetUpPlayStep1GO;
    [SerializeField] [NotNull] private GameObject SetUpPlayStep2GO;

    private void Awake()
    {
        MainMenuGO.SetActive(true);
        SetUpPlayStep1GO.SetActive(false);
        SetUpPlayStep2GO.SetActive(false);
    }

    private void Start()
    {
        AudioManager.Instance.Play("MainMenuBackgroundMusic");
    }

    public void GoToStep2()
    {
        MainMenuGO.SetActive(false);
        SetUpPlayStep1GO.SetActive(true);
        SetUpPlayStep2GO.SetActive(false);
    }

    public void GoToStep3(int numberOfTeams)
    {
        GlobalSettings.NumberOfTeams = numberOfTeams;
        
        MainMenuGO.SetActive(false);
        SetUpPlayStep1GO.SetActive(false);
        SetUpPlayStep2GO.SetActive(true);
    }

    public void GoToMainMenu()
    {
        MainMenuGO.SetActive(true);
        SetUpPlayStep1GO.SetActive(false);
        SetUpPlayStep2GO.SetActive(false);
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
