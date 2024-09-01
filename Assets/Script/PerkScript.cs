using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk")]
public class PerkScript : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public Image image;
    
    public int Cost;

    public bool defaultAvailable;


    public List<string> Effects;
}
