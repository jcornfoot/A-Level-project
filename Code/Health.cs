using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float hitPoints;
    [SerializeField] private GameObject bar;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private bool player;

    void Start() {
        healthBar = bar.GetComponent<HealthBar>();
        hitPoints = maxHealth;
        healthBar.UpdateHealthBar(hitPoints, maxHealth);
    }

    public void Hurt(float damage, int type) {
        if ((!player && type == 0) || (player && type == 1)) {
            hitPoints = Math.Max(hitPoints - damage, 0);
        healthBar.UpdateHealthBar(hitPoints, maxHealth);
        
        if (hitPoints == 0) {
            Die();
        }
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
