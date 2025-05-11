using System.Collections;
using UnityEngine;

public class ShootGunWeaponsController : MonoBehaviour, IWeapon
{
    [Header("Accuracy")] [SerializeField] private float accuracy = 0;

    [Header("Ammo")] [SerializeField] private int maxBulletsInClip;
    [SerializeField] private int maxBullets;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private int bulletsPerShoot = 5;

    [Header("Other")] [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private ParticleSystem muzzleParticle;
    [SerializeField] private AudioClip noBulletsClip;
    [SerializeField] private AudioClip bulletsClip;

    private bool _isPlayerOne;
    private LayerMask[] _targetMasks;

    private AudioSource _audioSource;
    private int _bulletsLeft;
    private int _carriedBulletsLeft;
    private bool _isFiring;
    private bool _canFire = true;
    private bool _isBot = false;

    private void Awake()
    {
        _bulletsLeft = maxBulletsInClip;
        _carriedBulletsLeft = maxBullets;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        muzzleParticle?.Stop();

        if (_isBot)
            EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
    }

    public void StartFire()
    {
        _isFiring = true;
    }

    public void EndFire()
    {
    }

    private void Update()
    {
        if (_isFiring && _canFire)
        {
            Fire();
            _isFiring = false;
        }
    }

    void Fire()
    {
        if (!HasBullets())
        {
            _audioSource.clip = noBulletsClip;
            _audioSource.Play();
            return;
        }

        for (int i = 0; i < bulletsPerShoot; i++)
        {
            float randomAngle = Random.Range(-accuracy, accuracy);
            GameObject go = Instantiate(bullet, firePoint.position,
                Quaternion.Euler(0, 0, firePoint.eulerAngles.z + randomAngle));
            var bulletController = go.GetComponent<BulletController>();
            bulletController?.AddTargetMasks(_targetMasks);
            bulletController?.ChangeDamage(damage);
        }

        _audioSource.clip = bulletsClip;
        _audioSource.Play();

        muzzleParticle?.Play();

        _bulletsLeft -= 1;

        if (_isBot)
            EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
    }

    public void Reload()
    {
        if (_bulletsLeft == maxBulletsInClip) return;

        if (_carriedBulletsLeft > 0)
            StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        if (_isBot)
        {
            EventsManager.Instance.ReloadStart(_isPlayerOne);
            AudioManager.Instance.Play("ReloadSound");
        }

        _canFire = false;

        yield return new WaitForSeconds(reloadTime);

        int amountToReload = maxBulletsInClip - _bulletsLeft;

        if (_carriedBulletsLeft - amountToReload > 0)
        {
            _carriedBulletsLeft -= amountToReload;
            _bulletsLeft = maxBulletsInClip;
        }
        else
        {
            _bulletsLeft += _carriedBulletsLeft;
            _carriedBulletsLeft = 0;
        }


        _canFire = true;

        if (_isBot)
        {
            EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
            EventsManager.Instance.ReloadEnd(_isPlayerOne);
        }
    }

    public void ReplenishAmmo()
    {
        _carriedBulletsLeft = maxBullets;
        if (_isBot)
            EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
    }

    public int CheckAmmo()
    {
        return _bulletsLeft;
    }

    public void SetIsBotFlag()
    {
        _isBot = true;
    }

    public void SetPlayer(bool isPlayerOne)
    {
        _isPlayerOne = isPlayerOne;
    }

    public void SetTargetMasks(LayerMask[] targetMasks)
    {
        _targetMasks = targetMasks;
    }

    private bool HasBullets()
    {
        return _bulletsLeft > 0;
    }
}
