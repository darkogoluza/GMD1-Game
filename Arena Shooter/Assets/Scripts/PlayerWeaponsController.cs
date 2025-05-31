using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponsController : MonoBehaviour
{
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform gunHands;

    [SerializeField] private Transform pistolHolder;
    [SerializeField] private Transform pistolHands;

    [SerializeField] public bool isPlayerOne;

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
        Gamepad gamepad = null;

        if (Gamepad.all.Count > (isPlayerOne ? 1 : 0))
        {
            gamepad = Gamepad.all[isPlayerOne ? 1 : 0];
        }

        if (gamepad == null)
            return;

        if (gamepad.buttonSouth.wasPressedThisFrame)
            _weapon?.StartFire();

        if (gamepad.buttonSouth.wasReleasedThisFrame)
            _weapon?.EndFire();

        if (gamepad.buttonNorth.wasPressedThisFrame)
            _weapon?.Reload(); 
    }

    public void SetNewTargetLayerMasks(LayerMask[] newTargetLayerMasks)
    {
        targetMasks = newTargetLayerMasks;
    }

    public void SetNewWeapon(bool isGun, GameObject weapon)
    {
        // Clear the holders
        foreach (Transform child in gunHolder.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in pistolHolder.transform)
        {
            Destroy(child.gameObject);
        }

        if (isGun)
        {
            gunHolder.gameObject.SetActive(true);
            gunHands.gameObject.SetActive(true);
            pistolHolder.gameObject.SetActive(false);
            pistolHands.gameObject.SetActive(false);

            GameObject gun = Instantiate(weapon, gunHolder);
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.identity;
            _weapon = gun.GetComponent<IWeapon>();
            _weapon?.SetPlayer(isPlayerOne);
            _weapon?.SetTargetMasks(targetMasks);
        }
        else
        {
            gunHolder.gameObject.SetActive(false);
            gunHands.gameObject.SetActive(false);
            pistolHolder.gameObject.SetActive(true);
            pistolHands.gameObject.SetActive(true);
            GameObject pistol = Instantiate(weapon, pistolHolder);
            pistol.transform.localPosition = Vector3.zero;
            pistol.transform.localRotation = Quaternion.identity;
            _weapon = pistol.GetComponent<IWeapon>();
            _weapon?.SetPlayer(isPlayerOne);
            _weapon?.SetTargetMasks(targetMasks);
        }
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
