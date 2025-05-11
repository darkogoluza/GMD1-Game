using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 30;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private float explosionDamage = 100;
    [SerializeField] private float radius = 10;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    private float _health = 0;
    private bool _isDamaged = false;
    private bool _isDead = false;

    private Collider2D[] _overlapResults = new Collider2D[100];

    private void Awake()
    {
        _health = maxHealth;
        smoke.Stop();
    }

    private void Update()
    {
        if (_isDamaged)
            TakeDamage(Time.deltaTime * 2);
    }

    public void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        _health -= damage;

        if (maxHealth * 0.5f >= _health && !_isDamaged)
        {
            _isDamaged = true;
            smoke.Play();
        }

        if (_health <= 0)
        {
            _isDead = true;
            Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 2f);
            AreaDamageEnemies(transform.position, radius, explosionDamage, targetMask);
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return _health;
    }

    private void AreaDamageEnemies(Vector2 location, float radius, float damage, LayerMask layer)
    {
        int count = Physics2D.OverlapCircleNonAlloc(location, radius, _overlapResults, layer);

        for (int i = 0; i < count; i++)
        {
            Collider2D col = _overlapResults[i];
            if (col != null)
            {
                if (col.transform == transform)
                    continue;

                LayerMask layerMask = col.gameObject.layer;

                if ((layer & (1 << layerMask)) != 0)
                {
                    Vector2 direction = ((Vector2) col.transform.position - location).normalized;
                    float distance = Vector2.Distance(location, col.transform.position);

                    RaycastHit2D hit = Physics2D.Raycast(location, direction, distance, obstacleMask);
                    if (hit.collider != null)
                        continue; // Skip this target if blocked

                    IDamageable enemy = col.GetComponent<IDamageable>();
                    if (enemy != null)
                    {
                        Vector2 dir = location - (Vector2) col.transform.position;
                        float proximity = dir.magnitude;
                        float effect = 1 - (proximity / radius);
                        effect = Mathf.Clamp(effect, 0.25f, 1);

                        enemy.TakeDamage(damage * effect);
                    }
                }
            }
        }
    }
}
