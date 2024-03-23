using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class Lights : MonoBehaviour
{
   public void Break () {
    GameObject child = gameObject.transform.parent.GetChild(1).gameObject;
    child.SetActive(true);
    Destroy(gameObject);
   }
}
