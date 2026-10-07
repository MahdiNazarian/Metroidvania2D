using UnityEngine;

public class SwingingBladeController : MonoBehaviour
{
    [Header("Swinging Blade Setting")]
    [SerializeField] private float speed;
    [SerializeField] private float maxAngle;
    private float timer;
    private float angle;

    [Header("Swinging Blade Knockback")]
    [SerializeField] private float bladeDamage;
    [SerializeField] private Vector2 knockBackForce;
    [SerializeField] private float knockBackDuration;

    private int pushDirection;
    private float previousAngle = 0.0f;
    void Update()
    {
        timer += speed * Time.deltaTime;
        angle = maxAngle * Mathf.Sin(timer);
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (angle > previousAngle)
            pushDirection = 1;
        else
            pushDirection = -1;

        previousAngle = angle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        KnockBackAblility knockBackAbility = collision.GetComponentInParent<KnockBackAblility>();

        knockBackAbility.StartSwingKnockBack(knockBackDuration, knockBackForce, pushDirection);
        playerStats.DamagePlayer(bladeDamage);
    }
}
