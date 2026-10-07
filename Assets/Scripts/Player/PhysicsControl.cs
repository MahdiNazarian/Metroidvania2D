using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    [Header("Coyote Timer")]
    [SerializeField] private float coyoteTime;
    [HideInInspector] public float coyoteTimer;

    [Header("Ground Check")]
    [SerializeField] private float groundDetectionRayLength;
    [SerializeField] private LayerMask groundLayerMask;

    private Transform groundCheckRight;
    private Transform groundCheckLeft;

    private RaycastHit2D resultGroundCheckLeft;
    private RaycastHit2D resultGroundCheckRight;

    public bool grounded;

    [Header("Wall Check")]
    [SerializeField] private float wallDetectionRayLength;
    [SerializeField] private LayerMask wallLayerMask;

    private Transform wallCheckUpper;
    private Transform wallCheckLower;

    private RaycastHit2D resultWallCheckUpper;
    private RaycastHit2D resultWallCheckLower;

    public bool wallDetected;

    [Header("Crouch")]
    [SerializeField] private Collider2D standCollider;
    [SerializeField] private Collider2D crouchCollider;

    [Header("Ceiling Check")]
    [SerializeField] private float ceilingDetectionRayLength;
    [SerializeField] private LayerMask ceilingLayerMask;
    private Transform ceilingCheckRight;
    private Transform ceilingCheckLeft;
    private RaycastHit2D resultCeilingCheckRight;
    private RaycastHit2D resultCeilingCheckLeft;

    public bool ceilingDetected;

    [Header("Interpolate")]
    [SerializeField] private RigidbodyInterpolation2D interpolate;
    [SerializeField] private RigidbodyInterpolation2D exterapolate;


    private float gravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        gravityScale = rb.gravityScale;
        groundCheckLeft = GameObject.Find("Player/GroundCheckLeft").transform;
        groundCheckRight = GameObject.Find("Player/GroundCheckRight").transform;
        wallCheckUpper = GameObject.Find("Player/WallCheckUpper").transform;
        wallCheckLower = GameObject.Find("Player/WallCheckLower").transform;
        ceilingCheckLeft = GameObject.Find("Player/CeilingCheckLeft").transform;
        ceilingCheckRight = GameObject.Find("Player/CeilingCheckRight").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!grounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = coyoteTime;
        }
    }

    private void FixedUpdate()
    {
        grounded = CheckGround();
        wallDetected = WallCheck();
        ceilingDetected = CeilingCheck();
    }

    private bool CheckGround()
    {
        resultGroundCheckLeft = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, groundDetectionRayLength, groundLayerMask);
        resultGroundCheckRight = Physics2D.Raycast(groundCheckRight.position, Vector2.down, groundDetectionRayLength, groundLayerMask);

        Debug.DrawRay(groundCheckLeft.position, new Vector3(0, -groundDetectionRayLength, 0), Color.red);
        Debug.DrawRay(groundCheckRight.position, new Vector3(0, -groundDetectionRayLength, 0), Color.red);

        return resultGroundCheckLeft || resultGroundCheckRight;
    }

    private bool WallCheck()
    {
        resultWallCheckUpper = Physics2D.Raycast(wallCheckUpper.position, transform.right, wallDetectionRayLength, wallLayerMask);
        resultWallCheckLower = Physics2D.Raycast(wallCheckLower.position, transform.right, wallDetectionRayLength, wallLayerMask);

        Debug.DrawRay(wallCheckUpper.position, new Vector3(wallDetectionRayLength, 0, 0), Color.red);
        Debug.DrawRay(wallCheckLower.position, new Vector3(wallDetectionRayLength, 0, 0), Color.red);

        return resultWallCheckUpper || resultWallCheckLower;
    }

    private bool CeilingCheck()
    {
        resultCeilingCheckLeft = Physics2D.Raycast(ceilingCheckLeft.position, Vector2.up, ceilingDetectionRayLength, ceilingLayerMask);
        resultCeilingCheckRight = Physics2D.Raycast(ceilingCheckRight.position, Vector2.up, ceilingDetectionRayLength, ceilingLayerMask);
        Debug.DrawRay(ceilingCheckLeft.position, new Vector3(0, ceilingDetectionRayLength, 0), Color.red);
        Debug.DrawRay(ceilingCheckRight.position, new Vector3(0, ceilingDetectionRayLength, 0), Color.red);
        return resultCeilingCheckLeft || resultCeilingCheckRight;
    }

    public float GetStartingGravity()
    {
        return gravityScale;
    }
    public void DisableGravity()
    {
        rb.gravityScale = 0;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityScale;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void activateCrouch()
    {
        crouchCollider.enabled = true;
        standCollider.enabled = false;
    }
    public void activateStand()
    {
        standCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    public void SetInterpolate()
    {
        rb.interpolation = interpolate;
    }
    public void SetExtrapolate()
    {
        rb.interpolation = exterapolate;
    }
}
