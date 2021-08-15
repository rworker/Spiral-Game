using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallTrigger : MonoBehaviour
{

    private bool alreadyTriggered = false;
    private PlayerController player;
    [SerializeField] Animator fadeAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyTriggered && other.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOut(other)); 
            alreadyTriggered = true; // prevents coroutine from being called twice

        }
    }

    public IEnumerator FadeOut (Collider other)
    {
        fadeAnimator.SetTrigger("FadeOut"); //this trigger causes animation in which BlackFade alpha goes from 0 to 1 in 2.1 seconds

        yield return new WaitForSeconds(2.5f);
        //yield return new WaitForSeconds(3f);
        //yield return new WaitUntil(() => fader.color.a >= 1);
        player = other.gameObject.GetComponent<PlayerController>();
        player.ableToMove = false; //makes it so charactercontroller.move is off and doesn't interfeor with teleportation to checkpoint

        //null conditional operator (checks if player and playerhearts are not null)
        player?.TakeDamage(4); //takes health of player from fall, if this kills player the gameover screen will automatically come up
        if (!player.playerHearts.IsDead()) //if player is not dead they will teleport to the last checkpoint then the fade in will happen
        {
            if (player.currentCheckpoint != null)
            {
                player.isInvincible = true; //makes it so enemies can't damage player while still faded out
                player.transform.position = player.currentCheckpoint.transform.position;
            }
            yield return new WaitForSeconds(0.5f); //makes sure adequate time exists for player to be teleported
            fadeAnimator.SetTrigger("FadeIn");
            player.ableToMove = true; //reenables ability to move
            player.isInvincible = false; //reenables player receiving damage
            alreadyTriggered = false; //reenables player to fall into same falltrigger after they've respawned
        }
        //20 = 5 hearts times 4 fragments each; used to automatically reduce hearts to 0 (would like to set damage value dynamically later on)
    }

    
}
