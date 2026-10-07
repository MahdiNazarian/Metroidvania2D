using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private HealthBarControl healthBarControl;
    private float currentHealth;
    private Player player;

    [Header("FlashEffect")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor;
    [SerializeField, Range(0,1)] private float flashStrength;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriter;
    private bool canTakeDamage = true;

    [Header("Colliders")]
    [SerializeField] private Collider2D standCollider;
    [SerializeField] private Collider2D crouchCollider;
    private Collider2D currentCollider;
    void Start()
    {
        currentHealth = playerMaxHealth;
        player = GetComponentInParent<Player>();
        healthBarControl.SetSliderValue(currentHealth, playerMaxHealth);
        spriter = GetComponentInParent<SpriteRenderer>();
        defaultMaterial = spriter.material;
        currentCollider = standCollider;
    }
    public void DamagePlayer(float damage)
    {
        if (!canTakeDamage)
            return;
        currentHealth -= damage;
        healthBarControl.SetSliderValue(currentHealth, playerMaxHealth);
        StartCoroutine(Flash());
        if (currentHealth <= 0)
        {
            DisableStatsCollider();
            if (player.stateMachine.currentState != PlayerStates.State.KnockBack)
                player.stateMachine.ChangeState(PlayerStates.State.Death);
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public bool IsAlive()
    {
        return currentHealth > 0;
    }
    public bool CanTakeDamage()
    {
        return canTakeDamage;
    }
    public void EnableTakeDamage()
    {
        canTakeDamage = true;
    }
    public void DisableTakeDamage()
    {
        canTakeDamage = false;
    }
    public void EnableStatsCollider()
    {
        currentCollider.enabled = true;
    }
    public void DisableStatsCollider()
    {
        currentCollider.enabled = false;
    }
    private IEnumerator Flash()
    {
        canTakeDamage = false;
        spriter.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashColor);
        flashMaterial.SetFloat("_FlashStrength", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        if (currentHealth > 0)
            canTakeDamage = true;
    }
    public void EnableStatsStandCollider()
    {
        if (currentHealth <= 0)
            return;
        crouchCollider.enabled = false;
        standCollider.enabled = true;
        currentCollider = standCollider;
    }
    public void EnableStatsCrouchCollider()
    {
        if (currentHealth <= 0)
            return;
        standCollider.enabled = false;
        crouchCollider.enabled = true;
        currentCollider = crouchCollider;
    }
}
