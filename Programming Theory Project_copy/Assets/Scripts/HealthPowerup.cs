using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : PowerUp
{
    [SerializeField] int healthBonus;
    

    public override void ApplyPowerup()
    {
        if (playerController.playerHearts.IsDamaged())
        {
            playerController.playerHearts.Heal(healthBonus);
        }
        else //adds a score bonus is player has full hearts and does not need the healing powerup
        {
            playerScore.AddtoScore(20);
        }

        base.ApplyPowerup(); //applies general powerup logic like deleting the powerup gameobject
    }
}
