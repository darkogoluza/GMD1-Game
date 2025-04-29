using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Properties")] [SerializeField]
    private float damage;

    [SerializeField] private float speed;
    [SerializeField] private float ricochetAngle = 60f;
    [SerializeField] private Transform impactPoint;

    [Space] [Header("Particles")] [SerializeField]
    protected GameObject impactParticle;

    [SerializeField] protected GameObject trailParticle;

    [Space] [Header("Mask Settings")] [SerializeField]
    private LayerMask[] targetMasks;

    private Vector2 _velocity = Vector2.zero;

    private GameObject _trailParticleRef;

    public void AddTargetMasks(LayerMask[] targetMasksToAdd)
    {
        targetMasks = targetMasks.Concat(targetMasksToAdd).ToArray();
    }
    
    public void ChangeDamage(float damage)
    {
        this.damage = damage;
    }

    void FixedUpdate()
    {
        CheckForCollision();
        Move();
    }

    private void Start()
    {
        SpawnTrail();
        SetVelocity(speed);
    }

    private bool CheckForCollision()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(impactPoint.position, impactPoint.transform.right,
            (speed * Time.fixedDeltaTime));
        if (hits != null)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform != null)
                {
                    foreach (var targetMask in targetMasks)
                    {
                        if (hit.transform.gameObject.layer == ToLayer(targetMask))
                        {
                            if (hit.collider != null)
                            {
                                Reflect(impactPoint.transform.right, hit.normal, hit);
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    private void SetVelocity(float strength)
    {
        Vector2 velocity = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),
            Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));

        _velocity = velocity * strength;
    }

    private void Impact(RaycastHit2D hit)
    {
        DetachTrail();
        DealDamage(hit);
        Destroy(gameObject);
        Destroy(Instantiate(impactParticle, transform.position, transform.rotation), 0.4f);
        AddForce(hit.transform, hit.point);
    }

    void DealDamage(RaycastHit2D hit)
    {
        hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
    }

    void AddForce(Transform target, Vector2 forcePosition)
    {
        if (target.GetComponent<Rigidbody2D>() != null)
        {
            Vector2 dir = forcePosition - (Vector2) target.position;
            target.GetComponent<Rigidbody2D>()
                .AddForceAtPosition(-dir * damage / 3f, forcePosition, ForceMode2D.Impulse);
        }
    }

    private void SpawnTrail()
    {
        if (trailParticle != null)
            _trailParticleRef = Instantiate(trailParticle, transform.position, Quaternion.identity, this.transform);
    }

    private void Move()
    {
        transform.position += (Vector3) _velocity * Time.fixedDeltaTime;
    }

    private void DetachTrail()
    {
        if (_trailParticleRef != null)
        {
            _trailParticleRef.transform.parent = null;
            Destroy(_trailParticleRef.gameObject, 0.5f);
        }
    }

    private void Reflect(Vector2 dir, Vector2 normal, RaycastHit2D hit)
    {
        Vector2 impactPointOffset =
            new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad),
                Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)) * impactPoint.localPosition.magnitude;
        transform.position = hit.point - impactPointOffset;
        float angleBetweenVectors = Vector2.Angle(-dir, normal);
        if (angleBetweenVectors > ricochetAngle)
        {
            Vector2 refDir = Vector2.Reflect(dir, normal);
            float angle = Mathf.Atan2(refDir.y, refDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
            SetVelocity(_velocity.magnitude);
            return;
        }

        Impact(hit);
    }

    private int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }

        return result;
    }
}
