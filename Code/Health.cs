using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float hitPoints;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject manager;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameManager gM;
    [SerializeField] private bool player;

    void Start() {
        healthBar = bar.GetComponent<HealthBar>();
        hitPoints = maxHealth;
        healthBar.UpdateHealthBar(hitPoints, maxHealth);
        gM = manager.GetComponent<GameManager>();
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
        if (player) gM.GameOver();
        Destroy(gameObject);
    }
}
