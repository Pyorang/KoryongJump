using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShrinkPotion : Item
{
    [SerializeField] private float shrinkSize;
    [SerializeField] private float shrinkTime;

    public override void Interact(GameObject Player) { Player.transform.localScale = new Vector2(shrinkSize, shrinkSize);}
    public float GetShrinkTime() {  return shrinkTime; }
    public void SettShrinkSize(float size) { shrinkSize = size; }
}
