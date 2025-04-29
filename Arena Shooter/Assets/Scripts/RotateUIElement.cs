using UnityEngine;

public class RotateUIElement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;

    private void Update()
    {
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
