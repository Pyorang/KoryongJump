using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterScript : ScriptableObject
{
    public string Name;
    public string Description;

    public Image image;

    public int cost;

    public List<int> perks;
    public List<string> Abilities;

    public bool defaultAvailable;
}
