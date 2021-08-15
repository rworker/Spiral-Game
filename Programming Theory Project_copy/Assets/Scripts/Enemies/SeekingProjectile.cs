using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private float despawnTime;
    [SerializeField] private int damageAmount;
    [SerializeField] private float speed;
    private Transform target;
    private Collider projectileCollider;

    //[Tooltip("From 0% to 100%")]
    //public float accuracy;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
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
    void Update()
    {
        target = player.gameObject.transform;

        transform.LookAt(target.transform);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private IEnumerator DespawnCountdown()
    {
        yield return new WaitForSeconds(despawnTime);
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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag != "Enemy")
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
