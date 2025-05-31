using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer flagAAreaSpriteRenderer;
    [SerializeField] private SpriteRenderer flagBAreaSpriteRenderer;
    [SerializeField] private SpriteRenderer flagCAreaSpriteRenderer;
    [SerializeField] private SpriteRenderer flagDAreaSpriteRenderer;
    [SerializeField] private SpriteRenderer flagEAreaSpriteRenderer;

    [Header("Team Colors")] [SerializeField]
    private Color teamOneColor = Color.blue;

    [SerializeField] private Color teamTwoColor = Color.red;
    [SerializeField] private Color teamThreeColor = Color.green;
    [SerializeField] private Color neutralColor = Color.grey;

    [Header("Capture Settings")] [SerializeField]
    private float baseCaptureRate = 0.1f; // per player per second

    [SerializeField] private float maxCaptureRate = 1f;

    [Header("Pulse Animation")] [SerializeField]
    private float pulseSpeed = 2f;

    [SerializeField] private float pulseStrength = 0.15f;

    public static FlagManager Instance;

    private enum FlagArea
    {
        A,
        B,
        C,
        D,
        E
    }

    private class FlagAreaState
    {
        public int teamOneCount = 0;
        public int teamTwoCount = 0;
        public int teamThreeCount = 0;
        public SpriteRenderer renderer;
        public float captureProgress;
        public float pulseTimer;
        public int currentOwner = 0;
    }

    private Dictionary<FlagArea, FlagAreaState> flagAreas;

    private void Awake()
    {
        Instance = this;

        flagAreas = new Dictionary<FlagArea, FlagAreaState>
        {
            { FlagArea.A, new FlagAreaState { renderer = flagAAreaSpriteRenderer } },
            { FlagArea.B, new FlagAreaState { renderer = flagBAreaSpriteRenderer } },
            { FlagArea.C, new FlagAreaState { renderer = flagCAreaSpriteRenderer } },
            { FlagArea.D, new FlagAreaState { renderer = flagDAreaSpriteRenderer } },
            { FlagArea.E, new FlagAreaState { renderer = flagEAreaSpriteRenderer } },
        };
    }

    public void OnTriggerEnter2DReceiver(char area, int teamId)
    {
        if (!TryGetFlagArea(area, out var state)) return;

        switch (teamId)
        {
            case 1: state.teamOneCount++; break;
            case 2: state.teamTwoCount++; break;
            case 3: state.teamThreeCount++; break;
            default:
                Debug.LogWarning("Invalid teamId: " + teamId);
                return;
        }
    }

    public void OnTriggerExit2DReceiver(char area, int teamId)
    {
        if (!TryGetFlagArea(area, out var state)) return;

        switch (teamId)
        {
            case 1: state.teamOneCount = Mathf.Max(0, state.teamOneCount - 1); break;
            case 2: state.teamTwoCount = Mathf.Max(0, state.teamTwoCount - 1); break;
            case 3: state.teamThreeCount = Mathf.Max(0, state.teamThreeCount - 1); break;
            default:
                Debug.LogWarning("Invalid teamId: " + teamId);
                return;
        }
    }
    
    public Dictionary<char, int> GetFlagOwners()
    {
        var result = new Dictionary<char, int>();

        foreach (var kvp in flagAreas)
        {
            char areaLabel = kvp.Key.ToString()[0]; 
            int ownerTeam = kvp.Value.currentOwner; 
            result[areaLabel] = ownerTeam;
        }

        return result;
    }
    
    private void Update()
    {
        foreach (var kvp in flagAreas)
        {
            var state = kvp.Value;
            int[] teamCounts = new[] { state.teamOneCount, state.teamTwoCount, state.teamThreeCount };
            int teamsPresent = teamCounts.Count(c => c > 0);
            int leadingTeam = GetLeadingTeam(teamCounts);
            bool isContested = teamsPresent > 1;

            if (!isContested && leadingTeam != 0)
            {
                float captureDelta = CalculateCaptureDelta(teamCounts, leadingTeam);
                state.captureProgress += captureDelta * Time.deltaTime;
                state.captureProgress = Mathf.Clamp01(state.captureProgress);

                state.pulseTimer += Time.deltaTime * pulseSpeed;
                float t = (Mathf.Sin(state.pulseTimer) * 0.5f + 0.5f) * pulseStrength;
                Color currentColor = GetTeamColor(state.currentOwner);
                Color targetColor = GetTeamColor(leadingTeam);
                state.renderer.color = Color.Lerp(currentColor, targetColor, t);

                if (state.captureProgress >= 1f && state.currentOwner != leadingTeam)
                {
                    state.currentOwner = leadingTeam;
                    state.captureProgress = 0f; 
                    state.pulseTimer = 0f;
                }
            }
            else
            {
                state.pulseTimer = 0f;
                state.captureProgress = 0f; 
                state.renderer.color = GetTeamColor(state.currentOwner);
            }
        }
    }

    private bool TryGetFlagArea(char area, out FlagAreaState state)
    {
        if (System.Enum.TryParse(area.ToString().ToUpper(), out FlagArea parsed) &&
            flagAreas.TryGetValue(parsed, out state))
        {
            return true;
        }

        Debug.LogWarning($"Invalid flag area: {area}");
        state = null;
        return false;
    }


    private int GetLeadingTeam(int[] counts)
    {
        int max = 0;
        int team = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] > max)
            {
                max = counts[i];
                team = i + 1;
            }
        }

        return team;
    }

    private float CalculateCaptureDelta(int[] counts, int leadingTeam)
    {
        float total = 0f;
        for (int i = 0; i < counts.Length; i++)
        {
            if (i + 1 == leadingTeam)
                total += counts[i];
            else
                total -= counts[i];
        }

        return Mathf.Clamp(total * baseCaptureRate, -maxCaptureRate, maxCaptureRate);
    }

    private Color GetTeamColor(int teamId)
    {
        return teamId switch
        {
            1 => teamOneColor,
            2 => teamTwoColor,
            3 => teamThreeColor,
            _ => neutralColor
        };
    }
}
