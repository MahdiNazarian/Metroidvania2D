using UnityEngine;

public class FirstBossProjectileController : MonoBehaviour
{
    [SerializeField] private string projectileDestroyAnimationName;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerStats playerStats))
        {
            playerStats.DamagePlayer(damage);
        }
        rb.linearVelocity = Vector2.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        animator.Play(projectileDestroyAnimationName);
    }

    public void MakeProjectile(Transform playerTransform)
    {
        if(playerTransform == null)
        {
            rb.linearVelocity = Vector2.down * speed;
            return;
        }
        Vector3 targetPosition = playerTransform.position + Vector3.up;
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.linearVelocity = direction * speed;

    }

    public void Destroyer()
    {
        Destroy(gameObject);
    }

}
