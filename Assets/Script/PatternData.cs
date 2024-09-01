using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern Data", menuName = "Scriptable Object/Pattern Data", order = int.MaxValue)]

public class PatternData : ScriptableObject
{
    [SerializeField] private int patternNum;
    [SerializeField] private List<PatternObj> objects;

    public List<PatternObj> GetObjectsList() { return objects; }
}