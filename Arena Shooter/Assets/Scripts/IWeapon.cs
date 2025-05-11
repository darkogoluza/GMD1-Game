using UnityEngine;

public interface IWeapon
{
    public void StartFire();
    public void EndFire();
    public void Reload();
    public void SetPlayer(bool isPlayerOne);
    public void SetTargetMasks(LayerMask[] targetMasks);
    public void ReplenishAmmo();
    public int CheckAmmo();
    public void SetIsBotFlag();
}
