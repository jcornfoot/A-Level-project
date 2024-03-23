using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.UI;
using UnityEngine;
/*Source:*/
/*https://www.youtube.com/watch?v=wkKsl1Mfp5M*/
public class Bullet : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float ProjectileSpeed = 20f;
    [SerializeField] private float ProjectileDamage = 20;
    [SerializeField] private Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.right * ProjectileSpeed;
    }

    void OnTriggerEnter2D (Collider2D Hit) {
        if (Hit.gameObject.name == "Light_ON") {
            Lights lights=Hit.GetComponent<Lights>();
            lights.Break();
        }
        Health enemy=Hit.GetComponent<Health>();
        if (enemy != null) {
            enemy.Hurt(ProjectileDamage, 0);
        }
        
        Destroy(gameObject);   
    }
    
}
