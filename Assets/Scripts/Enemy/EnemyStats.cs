using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;

    [Header("FlashEffect")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor;
    [SerializeField, Range(0, 1)] private float flashStrength;
    [SerializeField] private Material flashMaterial;
    private SpriteRenderer spriter;
    private Material defaultMaterial;

    private Coroutine damageCoroutine;


    void Start()
    {
        Initialize();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        DamageProcess();
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(Flash());
        if(health <= 0)
        {
            DeathProcess();
        }

    }

    protected virtual void Initialize()
    {
        spriter = GetComponentInParent<SpriteRenderer>();
        defaultMaterial = spriter.material;
    }

    protected virtual void DamageProcess()
    {

    }
    protected virtual void DeathProcess()
    {

    }

    private IEnumerator Flash()
    {
        spriter.material = flashMaterial;
        flashMaterial.SetColor("_FlashColor", flashColor);
        flashMaterial.SetFloat("_FlashStrength", flashStrength);
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        damageCoroutine = null;
    }

}
