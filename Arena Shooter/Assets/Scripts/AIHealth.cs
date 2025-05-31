using UnityEngine;

public class AIHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 20f;
    private EnemyUI _enemyUI;

    private void Awake()
    {
        _enemyUI = GetComponent<EnemyUI>();
        _enemyUI.SetMaxHealth(health);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        _enemyUI.UpdateHealth(health);
        if (health <= 0)
            Destroy(gameObject);
    }

    public float GetHealth()
    {
        return health;
    }
}
