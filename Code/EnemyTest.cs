using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public int Health = 100;

    void Die () {
        Destroy(gameObject);
    }
    public void Damaged (int Damage) {
        Health -= Damage;
        
        if (Health <= 0) {
            Die();
        }
    }
}
