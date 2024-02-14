using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Reference:
- https://www.youtube.com/watch?v=-YISSX16NwE
*/

public class WeaponManager : MonoBehaviour
{
    public int weaponCount;
    public int currentSlot;
    public GameObject[] weapons;
    public GameObject currentWeapon;


    void Start()
    {
        weaponCount = gameObject.transform.childCount;
        weapons = new GameObject[weaponCount];
        for (int i = 0; i< weaponCount; i++) {
            weapons[i] = gameObject.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
        currentWeapon = weapons[0];
        currentSlot = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            weapons[currentSlot].SetActive(false);
            
            if (currentSlot < weaponCount - 1) currentSlot ++;
            else if (currentSlot == weaponCount -1) currentSlot = 0;
            
            weapons[currentSlot].SetActive(true);
            currentWeapon = weapons[currentSlot];
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            weapons[currentSlot].SetActive(false);
            
            if (currentSlot > 0) currentSlot --;
            else if (currentSlot == 0) currentSlot = weaponCount -1;

            weapons[currentSlot].SetActive(true);
                currentWeapon = weapons[currentSlot];
        }
    }
}
