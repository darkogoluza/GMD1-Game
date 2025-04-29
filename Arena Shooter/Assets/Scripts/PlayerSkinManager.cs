using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private Skin skin;

    // Sprites
    [SerializeField] private SpriteRenderer backPackSpriteRenderer;
    [SerializeField] private SpriteRenderer headSpriteRenderer;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    [SerializeField] private SpriteRenderer leftArmGunSpriteRenderer;
    [SerializeField] private SpriteRenderer rightArmGunSpriteRenderer;
    [SerializeField] private SpriteRenderer leftArmPistolSpriteRenderer;
    [SerializeField] private SpriteRenderer rightArmPistolSpriteRenderer;

    private void Awake()
    {
        backPackSpriteRenderer.sprite = skin.BackPackSprite;
        headSpriteRenderer.sprite = skin.HeadSprite;
        bodySpriteRenderer.sprite = skin.BodySprite;
        leftArmGunSpriteRenderer.sprite = skin.LeftArmSprite;
        rightArmGunSpriteRenderer.sprite = skin.RightArmSprite;
        leftArmPistolSpriteRenderer.sprite = skin.LeftArmSprite;
        rightArmPistolSpriteRenderer.sprite = skin.LeftArmSprite;
    }
}
