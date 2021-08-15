using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    [SerializeField] private float despawnTime;
    public int damageAmount;
    [SerializeField] private float speed;
    //[SerializeField] private LayerMask projectileLayer;
    //[SerializeField] private LayerMask checkpointLayer;

    //[Tooltip("From 0% to 100%")]
    //public float accuracy;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public Collider projectileCollider;



    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(DespawnCountdown()); //starts countdown to despawn the object
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(muzzleVFX, ps.main.duration);
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
        projectileCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); //need to make this shoot properly, currently it will shoot on z axis
    }


    private IEnumerator DespawnCountdown()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag != "CheckPoint")
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

            if (other.gameObject.tag == "Enemy")
            {
                other.GetComponent<Enemy>().TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
        
    }
}
