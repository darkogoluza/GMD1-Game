using UnityEngine;

public class EntityInfo
{
    public LayerMask LayerMask;
    public bool IsPlayerOne;
    public bool IsBot;

    public EntityInfo(LayerMask layerMask, bool isPlayerOne, bool isBot)
    {
        LayerMask = layerMask;
        IsPlayerOne = isPlayerOne;
        IsBot = isBot;
    }
}
