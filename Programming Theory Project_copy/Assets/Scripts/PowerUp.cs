using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] string powerupName;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] bool expiresImmediately;
    public float durationSeconds;
    protected PlayerController playerController;
    protected ScoreTracker playerScore;


    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerScore = FindObjectOfType<ScoreTracker>();
    }

    /*private void OnCollisionEnter(Collision other) 
    {
        print("player");
        if (other.gameObject.CompareTag("Player"))
        {
            ApplyPowerup();
        }
    }*/


    public virtual void ApplyPowerup()
    {
        if (durationSeconds > 0)
        {
            playerController.powerupDuration = durationSeconds; // duration of powerup to equal the duration of the given powerup
        }
        Destroy(gameObject); //removes powerup from scene
    }

    //Applypowerup function that is overwritten in each subclass
    //apply powerup inlcudes deleting the powerup model
    //need logic for how powerup is picked up
}
