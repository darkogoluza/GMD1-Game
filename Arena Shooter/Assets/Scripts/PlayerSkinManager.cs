using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private Skin _skin;
    
    // Sprites
    [SerializeField] private SpriteRenderer _backPackSpriteRenderer;
    [SerializeField] private SpriteRenderer _headSpriteRenderer;
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;
    [SerializeField] private SpriteRenderer _leftArmGunSpriteRenderer;
    [SerializeField] private SpriteRenderer _rightArmGunSpriteRenderer;
    [SerializeField] private SpriteRenderer _leftArmPistolSpriteRenderer;
    [SerializeField] private SpriteRenderer _rightArmPistolSpriteRenderer;

    private void Awake()
    {
        _backPackSpriteRenderer.sprite = _skin.BackPackSprite;
        _headSpriteRenderer.sprite = _skin.HeadSprite;
        _bodySpriteRenderer.sprite = _skin.BodySprite;
        _leftArmGunSpriteRenderer.sprite = _skin.LeftArmSprite;
        _rightArmGunSpriteRenderer.sprite = _skin.RightArmSprite;
        _leftArmPistolSpriteRenderer.sprite = _skin.LeftArmSprite;
        _rightArmPistolSpriteRenderer.sprite = _skin.LeftArmSprite;
    }
}
