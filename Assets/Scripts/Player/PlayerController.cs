using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;
    
    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("References")]
    [SerializeField] private Transform planetCenter;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    
    private float currentRotation = 0f;
    private bool isGrounded = false;
    private Vector3 moveDirection;
    private float horizontalInput;
    private int combo = 0;
    private float health = 100f;
    
    public float Health => health;
    public int Combo => combo;
    
    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (planetCenter == null) planetCenter = FindObjectOfType<PlanetManager>().transform;
    }

    private void Update()
    {
        HandleInput();
        GroundCheck();
        
        if (horizontalInput != 0)
        {
            RotateAroundPlanet();
        }
    }

    private void FixedUpdate()
    {
        SpeedControl();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void RotateAroundPlanet()
    {
        currentRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        
        float radius = 8f;
        Vector3 newPosition = planetCenter.position + 
            new Vector3(Mathf.Cos(currentRotation * Mathf.Deg2Rad), 
                       0, 
                       Mathf.Sin(currentRotation * Mathf.Deg2Rad)) * radius;
        
        transform.position = newPosition;
        
        Vector3 direction = (newPosition - planetCenter.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheck.position, -transform.up, groundCheckDistance, groundLayer);
        
        rb.drag = isGrounded ? groundDrag : airDrag;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (flatVel.magnitude > 15f)
        {
            Vector3 limitedVel = flatVel.normalized * 15f;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        combo = 0;
        
        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void AddCombo()
    {
        combo++;
        GameManager.Instance.AddScore(10 * combo);
    }

    public void ResetCombo()
    {
        combo = 0;
    }
}