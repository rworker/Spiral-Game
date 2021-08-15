using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCoins : MonoBehaviour
{
    [SerializeField] private ScoreTracker playerScore;
    private Rigidbody playerRb;
    private int smallCoinWorth = 10;
    private int bigCoinWorth = 20;
    private bool alreadyPickedUp = false;

    private AudioSource playerAudio;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip bigCoinSound;
    [SerializeField] private AudioClip powerupSound;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
    }

    /*private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Hit");
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            playerScore.AddtoScore(smallCoinWorth);
        }
        if (other.gameObject.CompareTag("Big Coin"))
        {
            Destroy(other.gameObject);
            playerScore.AddtoScore(bigCoinWorth);
        }
    }
    */
   private void OnTriggerEnter(Collider other) 
   {
        if (other.gameObject.CompareTag("Coin") && !alreadyPickedUp)
        {
            playerAudio.PlayOneShot(coinSound);
            alreadyPickedUp = true;
            Destroy(other.gameObject);
            playerScore.AddtoScore(smallCoinWorth);
            alreadyPickedUp = false;
        }
        if (other.gameObject.CompareTag("Big Coin") && !alreadyPickedUp)
        {
            playerAudio.PlayOneShot(bigCoinSound);
            alreadyPickedUp = true;
            Destroy(other.gameObject);
            playerScore.AddtoScore(bigCoinWorth);
            alreadyPickedUp = false;
        }
        if(other.gameObject.CompareTag("Powerup"))
        {
            playerAudio.PlayOneShot(powerupSound);
            other.gameObject.GetComponent<PowerUp>().ApplyPowerup();
        }
   }

}
