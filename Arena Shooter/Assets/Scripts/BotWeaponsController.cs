using AI;
using JetBrains.Annotations;
using UnityEngine;

public class BotWeaponsController : MonoBehaviour
{
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform gunHands;

    [SerializeField] private Transform pistolHolder;
    [SerializeField] private Transform pistolHands;

    [SerializeField] private LayerMask[] targetMasks;

    [CanBeNull] private IWeapon _weapon;

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
            _weapon?.SetIsBotFlag();
            _weapon?.SetTargetMasks(targetMasks);
            GetComponent<AIAgent>().SetUpWeapon(_weapon);
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
            _weapon?.SetIsBotFlag();
            _weapon?.SetTargetMasks(targetMasks);
            GetComponent<AIAgent>().SetUpWeapon(_weapon);
        }
    }
}
