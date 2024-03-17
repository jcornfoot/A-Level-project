using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Reference:
- https://www.youtube.com/watch?v=-YISSX16NwE
*/

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private int weaponCount;
    public int currentSlot;
    private GameObject[] weapons;
    public GameObject currentWeapon;


    void Start()
    {
        weaponCount = gameObject.transform.childCount;
        weapons = new GameObject[weaponCount];
        for (int i = 0; i< weaponCount; i++) {
            weapons[i] = gameObject.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
        currentWeapon = weapons[0];
        currentSlot = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            weapons[currentSlot].SetActive(false);
            if (currentSlot < weaponCount - 1) currentSlot ++;
            else if (currentSlot == weaponCount -1) currentSlot = 0;
            currentWeapon = weapons[currentSlot];
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            weapons[currentSlot].SetActive(false); 
            if (currentSlot > 0) currentSlot --;
            else if (currentSlot == 0) currentSlot = weaponCount -1;
            currentWeapon = weapons[currentSlot];
        }
    }
}
