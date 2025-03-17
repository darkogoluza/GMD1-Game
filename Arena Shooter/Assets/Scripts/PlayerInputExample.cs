using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputExample : MonoBehaviour
{
    private Vector2 movement;
    private float speed = 5;
    public TextMeshPro text;

    void Update()
    {
        transform.Translate(movement);
    }
    
    public void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>()*Time.deltaTime*speed;
    }
    
    public void OnA()
    {
        text.text = "A";
    }
    
    public void OnB()
    {
        text.text = "B";
    }
    
    public void OnX()
    {
        text.text = "X";
    }
    
    public void OnY()
    {
        text.text = "Y";
    }
    
    public void OnLeftTrigger()
    {
        text.text = "L Trigger";
    }
    
    public void OnRightTrigger()
    {
        text.text = "R Trigger";
    }
    
    public void OnStart()
    {
        text.text = "Start";
    }
}
