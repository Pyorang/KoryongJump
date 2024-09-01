using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class FootHold : MonoBehaviour
{
    [SerializeField] protected float maxDistance = 0.5f;

    public bool CheckPlayerUp(GameObject player)
    {
        if (player.transform.position.y > this.transform.position.y)
            return true;
        else
            return false;
    }
}
