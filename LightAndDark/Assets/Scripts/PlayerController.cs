using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] float jumpForce = 3f;
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    private float _horizontalInput;
    private Vector2 horizontalMovement;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private bool isFacingRight = true;
    public bool isGrounded;
    public bool canJump;
    public GameObject leverPrefab;
    public bool canFlipLever = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    
    
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        else
        {
            _horizontalInput = Input.GetAxis("Horizontal") * speed;
            horizontalMovement = new Vector2(Input.GetAxis("Horizontal"), 0) * speed;
            rb.velocity = new Vector2(horizontalMovement.x, rb.velocity.y);
        }

        animator.SetFloat("Speed", Mathf.Abs(_horizontalInput));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   
        }

        /*if (Input.GetKeyDown(KeyCode.E) && canFlipLever)
        {
            leverController lever = leverPrefab.gameObject.GetComponent<leverController>();
            lever.Interact();
        }*/

        animator.SetBool("canJump", canJump);
        // Flip sprite if moving left
        if (_horizontalInput < 0)
        {
            isFacingRight = true;
            Flip(isFacingRight);
        }
        // Flip sprite if moving right
        else if (_horizontalInput > 0)
        {
            isFacingRight = false;
            Flip(isFacingRight);
        }
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //leverController lever = leverPrefab.gameObject.GetComponent<leverController>();
            Debug.Log("trying to interact with lever");
            lever.Interact(true);
        }
    }*/
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("lever"))
        {
            canFlipLever = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("lever"))
        {
            canFlipLever = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            canJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            canJump = true;

        }
    }

    
    public void Flip(bool isFacingRight)
    {
        if (!IsLocalPlayer) return;

        sr.flipX = isFacingRight;
        FlipSpriteServerRpc(isFacingRight);
    }
    [ServerRpc]
    public void FlipSpriteServerRpc(bool flip)
    {
        sr.flipX = flip;
        FlipSpriteClientRpc(flip);
    }

    [ClientRpc]
    public void FlipSpriteClientRpc(bool flip)
    {
        if (IsLocalPlayer) return;
        sr.flipX = flip;
    }

    
}