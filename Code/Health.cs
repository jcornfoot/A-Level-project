using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float hitPoints;
    [SerializeField] private HealthBar healthBar;

    void Start() {
        hitPoints = maxHealth;
        healthBar.UpdateHealthBar(hitPoints, maxHealth);
    }

    public void Hurt(float damage) {
        hitPoints = Math.Max(hitPoints - damage, 0);
        healthBar.UpdateHealthBar(hitPoints, maxHealth);
        
        if (hitPoints == 0) {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
