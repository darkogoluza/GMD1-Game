using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ak47WeaponController : MonoBehaviour, IWeapon
{
    [Header("Fire Rate")] [SerializeField] private int fireRate; // Bullets per minute

    [Header("Accuracy")] [SerializeField] private float accuracyMin = 0;
    [SerializeField] private float accuracyMax = 1;
    [SerializeField] private float accuracySpeedBuildUp = 1;
    [SerializeField] private float accuracySpeedCoolDown = 1;

    [Header("Ammo")] [SerializeField] private int maxBulletsInClip;
    [SerializeField] private int maxBullets;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private float damage = 1f;

    [Header("Other")] [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private ParticleSystem muzzleParticle;
    [SerializeField] private AudioClip noBulletsClip;
    [SerializeField] private AudioClip bulletsClip;

    private AudioSource _audioSource;
    private int _bulletsLeft;
    private int _carriedBulletsLeft;
    private float _fireRateTimer;
    private bool _isFiring = false;
    private bool _canFire = true;
    private float _accuracyPower = 0;
    private bool _isPlayerOne;
    private LayerMask[] _targetMasks;
    private bool _isBot = false;

    private void Awake()
    {
        _bulletsLeft = maxBulletsInClip;
        _carriedBulletsLeft = maxBullets;
        _fireRateTimer = 0;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        muzzleParticle?.Stop();

        if (!_isBot)
            EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
    }

    private void Update()
    {
        if (_isFiring && _canFire)
        {
            RepeatFire();
            _accuracyPower = Mathf.Clamp(_accuracyPower + Time.deltaTime * accuracySpeedBuildUp, 0, 1);
        }
        else
        {
            _accuracyPower = Mathf.Clamp(_accuracyPower - Time.deltaTime * accuracySpeedCoolDown, 0, 1);
        }

        _fireRateTimer -= Time.deltaTime;
    }

    void RepeatFire()
    {
        if (_fireRateTimer <= 0)
        {
            Fire();
            _fireRateTimer = 60f / fireRate;
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

        float accuracy = GetAccuracy();
        float randomAngle = Random.Range(-accuracy, accuracy);
        GameObject go = Instantiate(bullet, firePoint.position,
            Quaternion.Euler(0, 0, firePoint.eulerAngles.z + randomAngle));
        var bulletController = go.GetComponent<BulletController>();
        bulletController?.AddTargetMasks(_targetMasks);
        bulletController?.ChangeDamage(damage);

        _audioSource.clip = bulletsClip;
        _audioSource.Play();

        muzzleParticle?.Play();

        _bulletsLeft -= 1;

        EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
    }

    public void StartFire()
    {
        _isFiring = true;
    }

    public void EndFire()
    {
        _isFiring = false;
    }

    public void SetPlayer(bool isPlayerOne)
    {
        _isPlayerOne = isPlayerOne;
    }

    public void SetTargetMasks(LayerMask[] targetMasks)
    {
        _targetMasks = targetMasks;
    }

    public void ReplenishAmmo()
    {
        _carriedBulletsLeft = maxBullets;
        if (!_isBot)
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

    public void Reload()
    {
        if (_bulletsLeft == maxBulletsInClip) return;

        if (_carriedBulletsLeft > 0)
            StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        if (!_isBot)
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
        if (!_isBot)
        {
            EventsManager.Instance.AmmoChange(_bulletsLeft, _carriedBulletsLeft, _isPlayerOne);
            EventsManager.Instance.ReloadEnd(_isPlayerOne);
        }
    }


    private float GetAccuracy()
    {
        return Mathf.Lerp(accuracyMin, accuracyMax, _accuracyPower);
    }

    private bool HasBullets()
    {
        return _bulletsLeft > 0;
    }
}
