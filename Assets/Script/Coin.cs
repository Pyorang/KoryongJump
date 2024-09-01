using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public override void Interact(GameObject player)
    {
        if(!player.GetComponent<Player>().GetInvincible())
            player.GetComponent<Player>().Jump();
        player.GetComponent<Player>().EarnCoin(coin);
    }
}
