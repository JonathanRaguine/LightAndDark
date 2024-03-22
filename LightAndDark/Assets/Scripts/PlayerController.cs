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
    [SerializeField] private Transform spawnPoint;
    public AudioSource coinAudio;
    public AudioSource jumpAudio;
    public AudioSource runAudio;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //spawn player at spawnPoint
        transform.position = spawnPoint.transform.position;
    }
    

    private void Update()
    {   
        //allows only client to control player
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
        
        
        //jumps only if its touching ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   
            jumpAudio.Play();
        }
        

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

        bool pressKey = Input.GetAxis("Horizontal") != 0 && isGrounded;
        //only play footsteps if its moving and is grounded
        runAudio.enabled = pressKey;
    }

    //just for playing sounds when picking up coins
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("circle"))
        {
            coinAudio.Play();
        }
    }
    
    
    //ground check
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            canJump = false;
        }
    }
    
    
    //ground check
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            canJump = true;

        }
    }
    
    
    //flips sprite and sends to server
    public void Flip(bool isFacingRight)
    {
        if (!IsLocalPlayer) return;

        sr.flipX = isFacingRight;
        FlipSpriteServerRpc(isFacingRight);
    }
    //server call to flip sprite
    [ServerRpc]
    public void FlipSpriteServerRpc(bool flip)
    {
        sr.flipX = flip;
        FlipSpriteClientRpc(flip);
    }
    
    
    //client call to flip sprite
    [ClientRpc]
    public void FlipSpriteClientRpc(bool flip)
    {
        if (IsLocalPlayer) return;
        sr.flipX = flip;
    }

    
}