using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoes : Item
{
    [SerializeField] private float shoesOnTime;
    [SerializeField] private float shoesOnJumpPower;

    public override void Interact(GameObject player) { player.GetComponent<Player>().SetJumpPower(shoesOnJumpPower); }

    public float GetShoesOnTime() { return shoesOnTime; }
}
