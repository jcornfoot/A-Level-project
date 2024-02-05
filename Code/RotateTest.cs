using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    private Camera MainCam;
    private Vector3 mousePosition;
    void Start()
    {
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = MainCam.ScreenToWorldPoint(Input.mousePosition); //Vector to mouse position in world
        Vector3 rotation = mousePosition - transform.position; //Subtracts the vector to the firepoint position from the vector to the mouse postion
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg; //Works out the angle to point the firepoint to

        transform.rotation = Quaternion.Euler(0, 0, rotZ); //Points the firepoint to the correct angle
    }
}
