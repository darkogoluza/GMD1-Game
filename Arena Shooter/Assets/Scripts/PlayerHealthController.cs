using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    [Header("Health Settings")] [SerializeField]
    private float maxHealth = 100;

    [Header("Regeneration Settings")] [SerializeField]
    private float timeBeforeRegenStarts = 3f;

    [SerializeField] private float healthRegenSpeed = 15f;

    [Header("Other")] [SerializeField] public bool isPlayerOne;

    private float _currentHealth;
    private float _timeSinceLastDamage;
    private bool _isRegenerating = false;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    private void Start()
    {
        EventsManager.Instance.HealthChange(_currentHealth, maxHealth, isPlayerOne);
    }

    private void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;

        if (_timeSinceLastDamage >= timeBeforeRegenStarts)
        {
            RegenerateHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        _timeSinceLastDamage = 0f;
        _isRegenerating = false;

        EventsManager.Instance.HealthChange(_currentHealth, maxHealth, isPlayerOne);
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    private void RegenerateHealth()
    {
        if (_currentHealth >= maxHealth)
        {
            _currentHealth = maxHealth;
            return;
        }

        _currentHealth += healthRegenSpeed * Time.deltaTime;
        _currentHealth = Mathf.Min(_currentHealth, maxHealth);

        EventsManager.Instance.HealthChange(_currentHealth, maxHealth, isPlayerOne);
    }
}
