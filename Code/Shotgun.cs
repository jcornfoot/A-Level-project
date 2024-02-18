using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
[Header("Settings")]
    [SerializeField] private float fireRate;
    [SerializeField] private float shotCount;

    [Header("Accuracy")]
    [SerializeField] private float fireArc;
    [SerializeField] private float shotSpacing; /* degrees between each shot */

    
    

    [Header("Attachments")]
    [SerializeField] private GameObject bulletPrefab;
    private Camera mainCamera;
    private Vector3 mousePosition;
   
    private Transform firePoint;
    
    private float time;
    
    void Start()
    {
      mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      firePoint = gameObject.transform.Find("FirePoint");  
      fireRate = 1/fireRate;
      shotSpacing = fireArc/shotCount;
    }

    void OnEnable() {
    }

    void OnDisable() {
    }

    void Update()
    {
        /* Rotation */

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); /* Vector to mouse position in world */
        Vector3 rotation = mousePosition - transform.position; /* Subtracts the vector to the firepoint position from the vector to the mouse postion */
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; /* Works out the angle to point the firepoint to */

        transform.rotation = Quaternion.Euler(0, 0, rotZ); /* Points the object to the correct angle */
        

       /* Aiming */

        while(time > 0) {
            time -= Time.deltaTime;
            return;
        }
        

        if (Input.GetButton("Fire1") && time <= 0) {
            for (int i=0; i<shotCount; i++) {
                Fire();
                firePoint.rotation = Quaternion.Euler(0,0, rotZ + (fireArc - (2*i*shotSpacing)));
            }
            
            time = fireRate;
        }
        
        
    }

    void Fire() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
