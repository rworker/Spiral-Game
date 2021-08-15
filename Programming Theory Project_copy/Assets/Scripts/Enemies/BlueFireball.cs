using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFireball : FireBallProjectile
{
    private Transform target;
    private PlayerController player;

    private void Awake() 
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        target = player.transform;
        transform.LookAt(target); //should make fireball go towards player

        base.Start();
    }


    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "TriggerVolume" && other.gameObject.tag != "CheckPoint")
        {
            projectileCollider.enabled = false; //so object stops interacting with world (projectiles that are already destroyed won't block eachother)
            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, transform.position, transform.rotation) as GameObject;

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }

            if (other.gameObject.tag == "Player")
            {
                player.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}
