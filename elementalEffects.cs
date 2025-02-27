using UnityEngine;
using System.Collections;

public class ElementalEffect : MonoBehaviour
{
    public enum EffectType
    {
        InfernoPlague,
        // Add other effects here later...
    }

    public EffectType effectType;
    public float baseDamage = 10f; // Base damage per tick
    public float tickInterval = 1f; // Time between damage ticks
    public float duration = 3f; // Total duration of the effect
    public float fireStatBonus = 0f;
    public float toxicStatBonus = 0f;

    private float timer = 0f;
    private float effectTimer = 0f;
    private EnemyController enemyController;

    public void ApplyEffect(EnemyController enemy, float fireBonus, float toxicBonus)
    {
        Debug.Log("ElementalEffect ApplyEffect called!"); // Debugging
        enemyController = enemy;
        fireStatBonus = fireBonus;
        toxicStatBonus = toxicBonus;
        effectTimer = duration;
        Debug.Log($"Starting TickDamage coroutine. Duration: {duration}"); // Debugging
        StartCoroutine(TickDamage());
    }

    IEnumerator TickDamage()
{
    while (effectTimer > 0)
    {
        timer += Time.deltaTime;
        if (timer >= tickInterval)
        {
            float damage = baseDamage + (fireStatBonus + toxicStatBonus) / 10f;
            Debug.Log($"Inferno Plague: Dealt {damage} damage to {enemyController.enemyStats.enemyName}. Enemy Health: {enemyController.enemyCurrentHealth}"); // Debugging
            enemyController.TakeDamage(damage);
            timer = 0f;
        }
        effectTimer -= Time.deltaTime;
        yield return null;
    }
    Debug.Log("Inferno Plague effect finished.");
    Destroy(this);
}
}