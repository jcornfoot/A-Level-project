using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;



/* Source */
/* https://www.youtube.com/watch?v=6U_OZkFtyxY */
public class HealthBar : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image foreground;
    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        foreground.fillAmount = currentHealth / maxHealth;
    }
}
