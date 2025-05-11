using UnityEngine;

public class AIHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 20f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    public float GetHealth()
    {
        return health;
    }
}
