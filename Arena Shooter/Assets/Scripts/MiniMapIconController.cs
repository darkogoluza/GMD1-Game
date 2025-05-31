using UnityEngine;

public class MiniMapIconController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
    
    public void SetShape(Sprite newShape)
    {
        spriteRenderer.sprite = newShape;
    }
}
