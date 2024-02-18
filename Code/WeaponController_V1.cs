using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController_V1 : MonoBehaviour
{  
    [Header("Settings")]
    [SerializeField] private float fireRate;

    [Header("Accuracy")]
    [SerializeField] private float fireArc; /* innacuracy from the firepoint */
    [SerializeField] private float innacuracyModifier; /* the amount innacuracy increases towards fireArc per shot */
    
    
    [SerializeField] private float innacuracy; /* value between 0 & 1 to multiply innacuracy by so it ramps up with recoil */
    
    [SerializeField] private float accuracyReset; /* time for accuracy to reset */
    private float shotIncrement; /* value innacuracy is incremented by per shot  */
    private float spread; /* current spread of the shot */

    [Header("Attachments")]
    [SerializeField] private GameObject bulletPrefab;
    private Camera mainCamera;
    private Vector3 mousePosition;
   
    private Transform firePoint;
    
    private float time;
    private float cooldownTime;
    
    void Start()
    {
      mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      firePoint = gameObject.transform.Find("FirePoint");  
      fireRate = 1/fireRate;
      
    }

    void OnEnable() {
        shotIncrement = 1/(fireArc / innacuracyModifier);
    }

    void OnDisable() {
        innacuracy = 0;
        time = 0;
        cooldownTime = 0;
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
        if (Input.GetButtonDown("Fire1") && cooldownTime != 0) {
            cooldownTime = 0;
        }

        if (Input.GetButton("Fire1") && time <= 0) {
            spread = (Random.Range(-fireArc, fireArc) * innacuracy) + rotZ;
            firePoint.rotation = Quaternion.Euler(0,0, spread);
            Fire();
            time = fireRate;
            if (innacuracy < 1) innacuracy+= shotIncrement;
        }
        
        else {
            if (innacuracy != 0 && cooldownTime > accuracyReset) {
                innacuracy = 0;
                cooldownTime = 0;
            }
            else if (innacuracy != 0) {
                cooldownTime += Time.deltaTime;
            }
        }
    }

    void Fire() {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
