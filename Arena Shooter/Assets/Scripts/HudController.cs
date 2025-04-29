using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [Header("Ammo")] [SerializeField] private TextMeshProUGUI playerOneAmmoText;
    [SerializeField] private TextMeshProUGUI playerTwoAmmoText;

    [Header("Health")] [SerializeField] private TextMeshProUGUI playerOneHealthText;
    [SerializeField] private TextMeshProUGUI playerTwoHealthText;
    [SerializeField] private Slider playerOneHealthSlider;
    [SerializeField] private Slider playerTwoHealthSlider;
    [SerializeField] private Image playerOneDamageIndicator;
    [SerializeField] private Image playerTwoDamageIndicator;
    [SerializeField] private float gainDamageIndicationPower = 2f;
    [SerializeField] private float gainHealthIndicationPower = 4f;

    [Header("Reload")] [SerializeField] private GameObject playerOneReloadPanel;
    [SerializeField] private GameObject playerTwoReloadPanel;

    private float _playerOnePreviousHealth = 100f; // Value in percentages
    private float _playerTwoPreviousHealth = 100f; // Value in percentages

    private byte _playerOneDamageLevel = 0;
    private byte _playerTwoDamageLevel = 0;

    private void Awake()
    {
        playerOneReloadPanel.gameObject.SetActive(false);
        playerTwoReloadPanel.gameObject.SetActive(false);

        EventsManager.Instance.onAmmoChange += OnAmmoChange;
        EventsManager.Instance.onHealthChange += OnHealthChange;
        EventsManager.Instance.onReloadStart += OnReloadStart;
        EventsManager.Instance.onReloadEnd += OnReloadEnd;
    }

    private void OnAmmoChange(int newAmmo, int maxAmmo, bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            playerOneAmmoText.text = newAmmo + " / " + maxAmmo;
        }
        else
        {
            playerTwoAmmoText.text = newAmmo + " / " + maxAmmo;
        }
    }

    private void OnHealthChange(float newHealth, float maxHealth, bool isPlayerOne)
    {
        float healthToAssign = (newHealth / maxHealth) * 100f; // To convert into percentage

        if (isPlayerOne)
        {
            playerOneHealthText.text = Mathf.CeilToInt(healthToAssign) + "%";
            playerOneHealthSlider.value = healthToAssign;

            UpdateDamageIndicator(ref _playerOneDamageLevel, playerOneDamageIndicator, _playerOnePreviousHealth,
                healthToAssign, isPlayerOne: true);
            _playerOnePreviousHealth = healthToAssign;
        }
        else
        {
            playerTwoHealthText.text = Mathf.CeilToInt(healthToAssign) + "%";
            playerTwoHealthSlider.value = healthToAssign;

            UpdateDamageIndicator(ref _playerTwoDamageLevel, playerTwoDamageIndicator, _playerTwoPreviousHealth,
                healthToAssign, isPlayerOne: false);
            _playerTwoPreviousHealth = healthToAssign;
        }
    }

    private void OnReloadStart(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            playerOneReloadPanel.gameObject.SetActive(true);
        }
        else
        {
            playerTwoReloadPanel.gameObject.SetActive(true);
        }
    }

    private void OnReloadEnd(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            playerOneReloadPanel.gameObject.SetActive(false);
        }
        else
        {
            playerTwoReloadPanel.gameObject.SetActive(false);
        }
    }

    private void UpdateDamageIndicator(ref byte damageLevel, Image indicator, float previousHealth, float newHealth,
        bool isPlayerOne)
    {
        float healthChange = newHealth - previousHealth;

        if (healthChange < 0)
        {
            damageLevel = (byte) Mathf.Clamp(damageLevel + Mathf.CeilToInt(-healthChange * gainDamageIndicationPower),
                0, 255);
        }
        else if (healthChange > 0)
        {
            damageLevel = (byte) Mathf.Clamp(damageLevel - Mathf.CeilToInt(healthChange * gainHealthIndicationPower), 0,
                255);
        }

        var color = indicator.color;
        color.a = damageLevel / 255f;
        indicator.color = color;
    }
}
