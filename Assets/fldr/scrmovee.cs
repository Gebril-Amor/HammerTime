using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrmove : MonoBehaviour
{
    public float moveSpeed = 7f; // Adjust this in the Inspector for desired speed
    public float jumpForce = 10f; // Adjust this for desired jump height
    public Transform groundCheck; // An empty GameObject placed at the player's feet
    public LayerMask groundLayer; // Layer containing your ground/platform objects

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer); // Adjust radius as needed

        // Horizontal Movement
        float moveInput = Input.GetKey(KeyCode.Q) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f; // Q/D keys
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded) // Spacebar
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}