using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*Sources:*/
/*https://www.youtube.com/watch?v=wkKsl1Mfp5M*/
public class TestWeapon : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject ProjectilePrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Fire();
        }
    }

    void Fire () {
        Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
    }
}
