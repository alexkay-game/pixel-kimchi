using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlayer : MonoBehaviour
{
    public float speed;

    Vector2 direction;
    Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;               //vypnutí gravitace
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        float xInput = 0;
        float yInput = 0;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)    yInput += 1;
            if (Keyboard.current.sKey.isPressed)    yInput -= 1;
            if (Keyboard.current.aKey.isPressed)    xInput -= 1;
            if (Keyboard.current.dKey.isPressed)    xInput += 1;
        }

        direction = new Vector2(xInput, yInput);
        if (direction != Vector2.zero)
        {
            direction = direction.normalized;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = speed * direction;
    }
}
