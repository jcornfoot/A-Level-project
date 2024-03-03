using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
/*Source:*/
/*https://www.youtube.com/watch?v=wkKsl1Mfp5M*/
public class Bullet : MonoBehaviour
{
    [Header("Config")]
    public float ProjectileSpeed = 20f;
    public float ProjectileDamage = 20;
    public Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.right * ProjectileSpeed;
    }

    void OnTriggerEnter2D (Collider2D Hit) {
        Health enemy=Hit.GetComponent<Health>();
        if (enemy != null) {
            enemy.Hurt(ProjectileDamage);
        }
        Destroy(gameObject);   
    }
    
}
