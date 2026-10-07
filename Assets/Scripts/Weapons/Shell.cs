using UnityEngine;

public class Shell : MonoBehaviour
{
    private float verticalForce;
    private float horizontalForce;
    [SerializeField] private float minVerticalForce;
    [SerializeField] private float maxVerticalForce;
    [SerializeField] private float minHorizontalForce;
    [SerializeField] private float maxHorizontalForce;
    [SerializeField] private float lifeSpan;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        verticalForce = Random.Range(minVerticalForce, maxVerticalForce);
        horizontalForce = Random.Range(minHorizontalForce, maxHorizontalForce);

        Vector2 force = (Vector2)transform.up * verticalForce + (Vector2)transform.right * (-1) * horizontalForce;

        rb.AddForce(force, ForceMode2D.Impulse);

        Destroy(gameObject, lifeSpan);
    }

}
