using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    private bool isStarted = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            isStarted = true;
    }

    public bool GetStarted() { return isStarted; }
    public void ResetIsStarted() { isStarted = false; }
}
