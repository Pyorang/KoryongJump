using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatternObj
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Vector3 location;

    public GameObject GetObject() { return obj; }
    public Vector3 GetLocation() { return location; }
}
