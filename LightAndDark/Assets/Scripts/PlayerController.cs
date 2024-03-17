using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    private Rigidbody2D rb;


    private NetworkVariable<int> randNum = new NetworkVariable<int>(1);
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) Destroy(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 horizontalMovement = new Vector2(horizontalInput, 0f) * moveSpeed;

        // Apply horizontal movement
        rb.velocity = new Vector2(horizontalMovement.x, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}