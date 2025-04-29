using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public event Action<int, int, bool> onAmmoChange;
    public event Action<float, float, bool> onHealthChange;
    public event Action<bool> onReloadStart;
    public event Action<bool> onReloadEnd;

    public void AmmoChange(int newAmmo, int maxAmmo, bool isPlayerOne)
    {
        if (onAmmoChange != null)
            onAmmoChange(newAmmo, maxAmmo, isPlayerOne);
    }

    public void HealthChange(float newHealth, float maxHealth, bool isPlayerOne)
    {
        if (onHealthChange != null)
            onHealthChange(newHealth, maxHealth, isPlayerOne);
    }

    public void ReloadStart(bool isPlayerOne)
    {
        if (onReloadStart != null)
            onReloadStart(isPlayerOne);
    }

    public void ReloadEnd(bool isPlayerOne)
    {
        if (onReloadEnd != null)
            onReloadEnd(isPlayerOne);
    }
}
