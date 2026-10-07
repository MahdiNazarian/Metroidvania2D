using UnityEngine;

public class TakeDamageTest : MonoBehaviour, IDamagable
{
    private float maxHealth = 5.0f;
    public void TakeDamage(float damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
            Destroy(gameObject);
    }
}
