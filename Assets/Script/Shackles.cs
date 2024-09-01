using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shackles : Item
{
    [SerializeField] private float itemGravityScale;

    public float GetItemGravityScale() { return itemGravityScale; }
    public override void Interact(GameObject Player) { }

}
