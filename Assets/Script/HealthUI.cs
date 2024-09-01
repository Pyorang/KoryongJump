using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] heart;

    public void ChangeHealthPlusUI(int healthIndex) { heart[healthIndex - 1].color = new Color(255f/255f, 0f, 0f); }
    public void ChangeHealthMinusUI(int healthIndex) { heart[healthIndex - 1].color = new Color(70f/255f, 0f, 0f); }

}
