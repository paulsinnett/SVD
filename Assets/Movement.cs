using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5;
    public InputActionReference move;

    public void OnEnable()
    {
        move.action.Enable();
    }

    public void OnDisable()
    {
        move.action.Disable();
    }

    public void Update()
    {
        Vector2 movement = move.action.ReadValue<Vector2>();
        transform.Translate(speed * Time.deltaTime * new float3(movement.x, 0, movement.y), Space.World);
        
    }
}
