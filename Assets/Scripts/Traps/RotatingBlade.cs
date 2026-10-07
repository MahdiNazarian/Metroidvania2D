using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bladeDamage;
    [SerializeField] private Vector2 knockBackForce;
    [SerializeField] private float knockBackDuration;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        KnockBackAblility knockBackAbility = collision.GetComponentInParent<KnockBackAblility>();

        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform);
        playerStats.DamagePlayer(bladeDamage);
    }
}
