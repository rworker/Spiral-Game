using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    public event EventHandler OnLevelComplete;

    private bool alreadyTiggered = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (!alreadyTiggered && other.gameObject.tag == "Player")
        {
            //triggers if OnLevelComplete has subscribers
            if (OnLevelComplete != null) OnLevelComplete(this, EventArgs.Empty);
        }
    }

}
