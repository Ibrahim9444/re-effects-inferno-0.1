﻿using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    public float lifeTime;
    private float currentLifeTime;
    private float damage;
    private WeaponStatsSO.WeaponElementType weaponElement;
    private float fireDamage;
    private float toxicDamage;

    void Start()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);

        if (currentLifeTime <= 0)
        {
            destroyProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit enemy!");
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.TakeDamage(damage);

            // Get the active element combination from the WeaponController
            WeaponController weaponController = GetComponentInParent<WeaponController>();
            if (weaponController != null)
            {
                Debug.Log("WeaponController found!"); // Debugging
                WeaponController.ActiveElementCombination activeCombination = weaponController.GetActiveElementCombination();

                Debug.Log($"Active Combination: {activeCombination}"); // Debugging

                // Trigger the appropriate effect based on the combination
                switch (activeCombination)
                {
                    case WeaponController.ActiveElementCombination.FireFire:
                        Debug.Log("Applying FireFire effect!");
                        ApplyInfernoPlague(enemyController);
                        break;
                    case WeaponController.ActiveElementCombination.FireToxic:
                        Debug.Log("Applying FireToxic effect!");
                        ApplyInfernoPlague(enemyController);
                        break;
                    case WeaponController.ActiveElementCombination.None:
                        Debug.Log("No elemental effect to apply.");
                        break;
                    default:
                        Debug.Log("Unknown elemental combination.");
                        break;
                }
            }
            else
            {
                Debug.LogWarning("WeaponController not found in parent.");
            }

            destroyProjectile();
        }
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetEffect(WeaponStatsSO.WeaponElementType weaponElement)
    {
        this.weaponElement = weaponElement;
    }

    public void SetElementalDamage(float fireDamage, float toxicDamage)
    {
        this.fireDamage = fireDamage;
        this.toxicDamage = toxicDamage;
    }

    private void ApplyInfernoPlague(EnemyController enemy)
    {
        Debug.Log("ApplyInfernoPlague called!");
        Debug.Log($"Fire Damage: {fireDamage}, Toxic Damage: {toxicDamage}"); // Debugging
        ElementalEffect effect = enemy.gameObject.AddComponent<ElementalEffect>();
        effect.effectType = ElementalEffect.EffectType.InfernoPlague;
        effect.duration = (fireDamage + toxicDamage) / 100f;
        Debug.Log($"Effect duration: {effect.duration}");
        effect.ApplyEffect(enemy, fireDamage, toxicDamage);
    }

    void destroyProjectile()
    {
        Destroy(gameObject);
    }
}