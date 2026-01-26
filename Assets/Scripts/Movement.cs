using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector2 inputDirection;
    public Rigidbody2D rb;
    float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.y = Input.GetAxis("Vertical");

        rb.AddForce(inputDirection * moveSpeed, ForceMode2D.Force);

        if (rb.linearVelocity.magnitude > moveSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
    }
}
