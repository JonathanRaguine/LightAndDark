using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    public bool isGrounded;
    private Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer sr;
    private bool canJump = false;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) Destroy(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        isGrounded = false;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 horizontalMovement = new Vector2(horizontalInput, 0f) * moveSpeed;
        animator.SetFloat("Speed",Mathf.Abs(horizontalInput * moveSpeed));
        if (horizontalInput > 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        // Apply horizontal movement
        rb.velocity = new Vector2(horizontalMovement.x, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isGrounded", true);
        }
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("circle"))
        {
            var networkObject = other.gameObject.GetComponent<NetworkObject>();
            ObjectSpawner.Instance.DespawnObjectServerRpc(networkObject.NetworkObjectId);
            Debug.Log("touching circle");
        }

    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = true;

            Debug.Log("touching ground!");
            animator.SetBool("isGrounded", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            Debug.Log("jumping");
        }
    }
}