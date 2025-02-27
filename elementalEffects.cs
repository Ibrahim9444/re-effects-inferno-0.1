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
    public float fireStatBonus = 0f;
    public float toxicStatBonus = 0f;

    private float timer = 0f;
    private float effectTimer = 5f; // Fixed duration of 5 seconds
    private EnemyController enemyController;

    public void ApplyEffect(EnemyController enemy, float fireBonus, float toxicBonus)
    {
        Debug.Log("ElementalEffect ApplyEffect called!"); // Debugging
        enemyController = enemy;
        fireStatBonus = fireBonus;
        toxicStatBonus = toxicBonus;
        effectTimer = 5f; //Fixed duration
        Debug.Log($"Starting TickDamage coroutine. Duration: {effectTimer}"); // Debugging
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
                float damageMultiplier = GetDamageMultiplier(enemyController.enemyStats.enemyType); // Get multiplier
                damage *= damageMultiplier; // Apply multiplier
                Debug.Log($"Inferno Plague: Dealt {damage} damage to {enemyController.enemyStats.enemyName}. Enemy Health: {enemyController.enemyCurrentHealth}, Multiplier: {damageMultiplier}"); // Debugging
                enemyController.TakeDamage(damage);
                timer = 0f;
            }
            effectTimer -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Inferno Plague effect finished.");
        Destroy(this);
    }

    private float GetDamageMultiplier(EnemyStatsSO.EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyStatsSO.EnemyType.Parasite:
            case EnemyStatsSO.EnemyType.Mutant:
                return 1.2f; // Effective
            case EnemyStatsSO.EnemyType.Armored:
            case EnemyStatsSO.EnemyType.Cybernetic:
                return 0.6f; // Weak
            default:
                return 0.8f; // Neutral
        }
    }
}