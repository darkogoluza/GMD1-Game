using JetBrains.Annotations;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    [SerializeField] private Transform gunHolder;

    [SerializeField] private bool isPlayerOne;

    [SerializeField] private LayerMask[] targetMasks;

    [CanBeNull] private IWeapon _weapon;

    private void Awake()
    {
        _weapon = gunHolder.GetChild(0).GetComponent<IWeapon>();
        _weapon?.SetPlayer(isPlayerOne);
        _weapon?.SetTargetMasks(targetMasks);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _weapon?.StartFire();

        if (Input.GetKeyUp(KeyCode.Space))
            _weapon?.EndFire();

        if (Input.GetKeyDown(KeyCode.R))
            _weapon?.Reload();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AmmoPickUp"))
        {
            Destroy(other.gameObject);
            _weapon?.ReplenishAmmo();
            AudioManager.Instance.Play("AmmoPickUp");
        }
    }
}
