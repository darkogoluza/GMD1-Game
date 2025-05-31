using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUIController : MonoBehaviour
{
    [SerializeField] private GameObject teamThree;
    [SerializeField] private Slider teamOneSlider;
    [SerializeField] private Slider teamTwoSlider;
    [SerializeField] private Slider teamThreeSlider;
    [SerializeField] private TextMeshProUGUI teamOneProgressText;
    [SerializeField] private TextMeshProUGUI teamTwoProgressText;
    [SerializeField] private TextMeshProUGUI teamThreeProgressText;

    [Header("Team Progress")] [SerializeField]
    private float progressGainRate = 1f;

    private float teamOneProgress = 0f;
    private float teamTwoProgress = 0f;
    private float teamThreeProgress = 0f;

    private bool isOver = false;

    private EndGameMenu _endGameMenu;

    private void Awake()
    {
        if (GlobalSettings.NumberOfTeams == 2)
        {
            teamThree.SetActive(false);
        }

        _endGameMenu = GetComponent<EndGameMenu>();
    }

    private void Update()
    {
        UpdateTeamProgress();

        teamOneSlider.value = teamOneProgress / 100f;
        teamTwoSlider.value = teamTwoProgress / 100f;
        teamThreeSlider.value = teamThreeProgress / 100f;

        teamOneProgressText.text = ((int) teamOneProgress).ToString(CultureInfo.InvariantCulture);
        teamTwoProgressText.text = ((int) teamTwoProgress).ToString(CultureInfo.InvariantCulture);
        teamThreeProgressText.text = ((int) teamThreeProgress).ToString(CultureInfo.InvariantCulture);
    }

    private void UpdateTeamProgress()
    {
        if(isOver)
            return;
        
        Dictionary<char, int> owners = FlagManager.Instance.GetFlagOwners();

        foreach (var kvp in owners)
        {
            switch (kvp.Value)
            {
                case 1:
                    teamOneProgress += progressGainRate * Time.deltaTime;
                    break;
                case 2:
                    teamTwoProgress += progressGainRate * Time.deltaTime;
                    break;
                case 3:
                    teamThreeProgress += progressGainRate * Time.deltaTime;
                    break;
            }
        }

        teamOneProgress = Mathf.Min(teamOneProgress, 100f);
        teamTwoProgress = Mathf.Min(teamTwoProgress, 100f);
        teamThreeProgress = Mathf.Min(teamThreeProgress, 100f);

        if (teamOneProgress >= 100f)
        {
            _endGameMenu.OpenEndGameMenu(1);
            isOver = true;
        }
        else if (teamTwoProgress >= 100f)
        {
            _endGameMenu.OpenEndGameMenu(2);
            isOver = true;
        }
        else if (teamThreeProgress >= 100f)
        {
            _endGameMenu.OpenEndGameMenu(3);
            isOver = true;
        }
    }

    public float GetTeamProgress(int teamId)
    {
        return teamId switch
        {
            1 => teamOneProgress,
            2 => teamTwoProgress,
            3 => teamThreeProgress,
            _ => 0f
        };
    }
}
