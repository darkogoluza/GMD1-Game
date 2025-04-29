using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _movement;
        
        if (_movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }

    public void OnMovement(InputValue value)
    {
        _movement = value.Get<Vector2>().normalized * movementSpeed;
    }
}
