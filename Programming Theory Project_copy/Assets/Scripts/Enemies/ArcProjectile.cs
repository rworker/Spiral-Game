using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectile : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private int damageAmount;
    [SerializeField] private float despawnTime;
    private Transform target;
    private float speed = 15;
    private const float DefaultGravity = -9.8f;
    private bool hit = false;

    private Rigidbody projectileRb;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        projectileRb = GetComponent<Rigidbody>();
        target = player.transform;
        
        LaunchProjectile();
    }

    private void Update() 
    {
        if (!hit)
        {
            transform.Rotate(0, 0, -1500 * Time.deltaTime);
        }
        else
        {
            projectileRb.freezeRotation = true;
        }
        
    }



    private void OnCollisionEnter(Collision other) //on collision since a rigidbody is involved
    {
        
        if (other.gameObject.tag == "Player") //make it so player can't walk into the object and still get hurt if it is on the ground
        {
            if (player.playerHearts != null) //avoids null reference error
            {
                player?.TakeDamage(damageAmount); //avoids null reference error
            }
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Ground")
        {
            StartCoroutine(DespawnCountdown());
        }

        if (other.gameObject.tag == "Enemy")
        {
            //Physics.IgnoreCollision()
        }
        hit = true;
    }

    private IEnumerator DespawnCountdown()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }


    private void LaunchProjectile()
    {
        Vector3 toTarget = target.position - transform.position;

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // Check whether the target is reachable at max speed or less.
        if (discriminant < 0)
        {
            Destroy(gameObject);
        }

        float discRoot = Mathf.Sqrt(discriminant);

        // Highest shot with the given max speed:
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

        // Most direct shot with the given max speed:
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

        // Lowest-speed arc available:
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f / gSquared));

        float T = T_max;// choose T_max, T_min, or some T in-between like T_lowEnergy

        // Convert from time-to-hit to a launch velocity:
        Vector3 velocity = toTarget / T - Physics.gravity * T / 2f;

        // Apply the calculated velocity (do not use force, acceleration, or impulse modes)
        projectileRb.AddForce(velocity, ForceMode.VelocityChange);
    }

    /*public bool SetTrajectory(Rigidbody rigidbody, Vector3 target, float force, float arch = 0.5f)
    {
        arch = Mathf.Clamp(arch, 0, 1);
        var origin = rigidbody.position;
        float x = target.x - origin.x;
        float y = target.y - origin.y;
        float gravity = -Physics.gravity.y;
        if (gravity == 0)
        {
            var constantForce = rigidbody.GetComponent<ConstantForce>();
            if (!constantForce)
            {
                Debug.LogWarning("There is no gravity and " + rigidbody.name + " does not have a ConstantForce attached.  A ConstantForce with the default gravity of -9.8f will be added.");
                constantForce = rigidbody.gameObject.AddComponent<ConstantForce>();
                constantForce.force = new Vector3(0, DefaultGravity, 0);
            }
            gravity = -constantForce.force.y;
        }
        float b = force * force - y * gravity;
        float discriminant = b * b - gravity * gravity * (x * x + y * y);
        discriminant = Mathf.Max(0, discriminant);
        float discriminantSquareRoot = Mathf.Sqrt(discriminant);
        float minTime = Mathf.Sqrt((b - discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
        float maxTime = Mathf.Sqrt((b + discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
        float time = (maxTime - minTime) * arch + minTime;
        float vx = x / time;
        float vy = y / time + time * gravity / 2;
        var trajectory = new Vector3(vx, vy, 0);
        trajectory = Vector3.ClampMagnitude(trajectory, force);
        rigidbody.AddForce(trajectory, ForceMode.Impulse);
        return true;
    }*/

}
