using System;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private Char flagChar;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInLayerMask(other.gameObject, GameManager.Instance.teamOneLayerMask))
        {
            FlagManager.Instance.OnTriggerEnter2DReceiver(flagChar, 1);
        }
        else if (IsInLayerMask(other.gameObject, GameManager.Instance.teamTwoLayerMask))
        {
            FlagManager.Instance.OnTriggerEnter2DReceiver(flagChar, 2);
        }
        else if (IsInLayerMask(other.gameObject, GameManager.Instance.teamThreeLayerMask))
        {
            FlagManager.Instance.OnTriggerEnter2DReceiver(flagChar, 3);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsInLayerMask(other.gameObject, GameManager.Instance.teamOneLayerMask))
        {
            FlagManager.Instance.OnTriggerExit2DReceiver(flagChar, 1);
        }
        else if (IsInLayerMask(other.gameObject, GameManager.Instance.teamTwoLayerMask))
        {
            FlagManager.Instance.OnTriggerExit2DReceiver(flagChar, 2);
        }
        else if (IsInLayerMask(other.gameObject, GameManager.Instance.teamThreeLayerMask))
        {
            FlagManager.Instance.OnTriggerExit2DReceiver(flagChar, 3);
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
