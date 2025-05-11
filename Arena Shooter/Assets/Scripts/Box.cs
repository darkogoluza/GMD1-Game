using UnityEngine;

public class Box : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 30;
    [SerializeField] private GameObject explosionEffect;
    private float _health = 0;


    private void Awake()
    {
        _health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 0.5f);
            Destroy(gameObject); 
        }
    }

    public float GetHealth()
    {
        return _health;
    }
}
