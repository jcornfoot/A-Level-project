using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController_V1 : MonoBehaviour
{  
    private Camera mainCamera;
    private Vector3 mousePosition;

    public float fireRate = 2f;
    public float time;
    public GameObject bulletPrefab;
    private Transform firePoint;

    void Start()
    {
      mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      firePoint = gameObject.transform.Find("FirePoint");  
      fireRate = 1/fireRate;
    }

    void Update()
    {
        /* Rotation */

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); /* Vector to mouse position in world */
        Vector3 rotation = mousePosition - transform.position; /* Subtracts the vector to the firepoint position from the vector to the mouse postion */
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; /* Works out the angle to point the firepoint to */

        transform.rotation = Quaternion.Euler(0, 0, rotZ); /* Points the firepoint to the correct angle */
       
       /* Firing */

        while(time > 0) {
            time -= Time.deltaTime;
            return;
        }

        if (Input.GetButton("Fire1") && time <= 0) {
            Fire();
            time = fireRate;
        }
    }

    void Fire() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
