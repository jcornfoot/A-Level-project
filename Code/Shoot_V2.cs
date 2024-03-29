using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_V2 : MonoBehaviour
{
    public float fireRate = 0.5f; /* Time in seconds between shots */
    public float time;
    public GameObject bulletPrefab;

    void Update()
    {
        while (time > 0) {
            time -= Time.deltaTime;
            return;
        }
        if (Input.GetButton("Fire1") && time<=0) {
            Fire();
            time = fireRate;
        }
    }

    void Fire() {
        Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
