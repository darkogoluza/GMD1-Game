using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;
    private Gamepad assignedGamepad;


    public void AssignControllers(bool isPlayerOne)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        var gamepads = Gamepad.all;
        if (gamepads.Count > 0)
        {
            int index = isPlayerOne ? 1 : 0;
            if (index < gamepads.Count)
            {
                assignedGamepad = gamepads[index];
            }
        }
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

    private void Update()
    {
        if (assignedGamepad == null)
            return;

        Vector2 input = assignedGamepad.leftStick.ReadValue();
        _movement = input.normalized * movementSpeed;
    }
}
