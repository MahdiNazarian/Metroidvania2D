using UnityEngine;

public class RangeEnemyPhysiscsController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    [Header("Detect Wall")]
    [SerializeField] private Transform wallDetectionPoint;
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private LayerMask wallDetectionLayerMask;
    public bool wallDetected;

    [Header("Detect Ground")]
    [SerializeField] private Transform groundDetectionPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundDetectionLayerMask;
    public bool groundDetected;


    [Header("Colliders")]
    [SerializeField] private Collider2D statsCollider;

    [Header("Player Check")]
    [Header("Player Check Front")]
    [SerializeField] private Transform frontCheckPoint;
    [SerializeField] private float frontCheckLength;
    public bool playerFront;

    [Header("Player Check Behind")]
    [SerializeField] private Transform behindCheckPoint;
    [SerializeField] private float behindCheckLength;
    public bool playerBehind;
    public bool canCheckBehind = true;

    [SerializeField] private LayerMask playerLayerToDetect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        groundDetected = Physics2D.OverlapCircle(groundDetectionPoint.position, groundCheckRadius, groundDetectionLayerMask);
        wallDetected = Physics2D.OverlapCircle(wallDetectionPoint.position, wallCheckRadius, wallDetectionLayerMask);
        playerFront = Physics2D.Raycast(frontCheckPoint.position, transform.right, frontCheckLength, playerLayerToDetect);
        if (canCheckBehind)
            playerBehind = Physics2D.Raycast(behindCheckPoint.position, transform.right * (-1), behindCheckLength, playerLayerToDetect);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundDetectionPoint.position, groundCheckRadius);
        Gizmos.DrawWireSphere(wallDetectionPoint.position, wallCheckRadius);
        Debug.DrawLine(behindCheckPoint.position, behindCheckPoint.position + transform.right * (-1) * behindCheckLength);
        Debug.DrawLine(frontCheckPoint.position, frontCheckPoint.position + transform.right * frontCheckLength);
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void DisableDeathColliders()
    {
        statsCollider.enabled = false;
    }
}
