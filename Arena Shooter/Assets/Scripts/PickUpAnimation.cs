using UnityEngine;

public class PickUpAnimation : MonoBehaviour
{
    [Header("Wiggle Settings")]
    public float speed = 5f;            // Speed of the wiggle
    public float angle = 15f;           // Maximum rotation angle in degrees
    public int wiggleCount = 10;        // How many wiggles per cycle
    public float waitDuration = 2f;     // How long to wait between wiggle cycles (in seconds)

    private float timer = 0f;
    private int currentWiggles = 0;
    private bool isWiggling = true;
    private float initialZ;
    private float timeSinceLastWiggle = 0f;

    void Start()
    {
        initialZ = transform.localEulerAngles.z;
    }

    void Update()
    {
        if (isWiggling)
        {
            timer += Time.deltaTime;
            float zRotation = Mathf.Sin(timer * speed) * angle;

            Vector3 newRotation = transform.localEulerAngles;
            newRotation.z = initialZ + zRotation;
            transform.localEulerAngles = newRotation;

            // Count wiggles based on full sine wave cycles (π = half, 2π = full)
            if (timer * speed >= (currentWiggles + 1) * Mathf.PI * 2)
            {
                currentWiggles++;
                if (currentWiggles >= wiggleCount)
                {
                    isWiggling = false;
                    timer = 0f;
                    currentWiggles = 0;
                }
            }
        }
        else
        {
            timeSinceLastWiggle += Time.deltaTime;
            if (timeSinceLastWiggle >= waitDuration)
            {
                isWiggling = true;
                timeSinceLastWiggle = 0f;
            }

            // Hold rotation steady during wait
            Vector3 holdRotation = transform.localEulerAngles;
            holdRotation.z = initialZ;
            transform.localEulerAngles = holdRotation;
        }
    }
}
