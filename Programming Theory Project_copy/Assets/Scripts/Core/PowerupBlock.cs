using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBlock : MonoBehaviour
{
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] Transform spawnLocation;
    [SerializeField] ParticleSystem explosionParticle;
    private bool alreadyTriggered = false;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            if (!alreadyTriggered)
            {
                SpawnRandomPowerup();
                Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                other.gameObject.GetComponent<PlayerController>().direction.y = -1;
                Destroy(gameObject);
            }
        }
    }

    private void SpawnRandomPowerup()
    {
        int powerupIndex = Random.Range(0, powerUps.Count);

        Instantiate(powerUps[powerupIndex], spawnLocation.position, powerUps[powerupIndex].transform.rotation);

        alreadyTriggered = true;

        //doTween to make the projectile slide into existence?
    }

    
}
