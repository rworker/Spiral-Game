using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    private Animator playerAnim;

    [SerializeField] private float speed = 8f;
    public float jumpForce = 10f;
    [SerializeField] private float turnSpeed;
    private float horizontal;
    public float fireballFireRate;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform castPosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private GameObject fireballProjectile;
    [SerializeField] private Image fireballUI;
    [SerializeField] private Image juggernautUI;

    public bool isInvincible = false;
    public bool hasFirePowerup = false;
    //private int invincibilityDamage = 50;
    public float powerupDuration;
    //private float fireballCastTime = 0.3f;
    private float timeSincePowerup = 0;
    public float timeSinceFireball = 0; //so can be accessed in fireball powerup clas
    public Transform currentCheckpoint = null; //player's current checkpoint they respawn back at if they fall, initialized as null by default

    
    private float gravity = -20f;
    private bool isGrounded;
    private bool isOnPlatform;
    public Vector3 direction;
    private float turnDirectionMultiplier = -1f; //used to make sure player rotates in the direction of towards the camera

    private bool initiatedJump;
    private bool canDoubleJump;
    public HeartsSystem playerHearts;
    public int numberOfHearts = 4;
    
    public bool ableToMove = true; //by default player is able to move
    public Animator animator;

    public AudioSource playerAudio;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip doubleJumpSound;
    [SerializeField] private AudioClip castSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip losePowerupSound;
    public AudioClip deathSound;
 
    private void Awake() 
    {
        playerHearts = new HeartsSystem(numberOfHearts); //must be in awake so the heartvisual script can then reference playerhearts in its start method
    }
    // Start is called before the first frame update
    void Start()
    {
        //playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update() 
    {
        CheckForJump();
        CheckAndHandlePowerups();
        animator.SetFloat("speed", Mathf.Abs(horizontal));
        
    }

    void FixedUpdate() //fixed update is dedicated to handling physics so it is good for movement
    {
        if (ableToMove)
        {
            HandleMovement();
            HandleJump();
        }
    }

    void HandleMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (isGrounded || isOnPlatform)
        {
            //animator.SetFloat("speed", Mathf.Abs(horizontal)); //only transitions to running if on ground (doesn't transition mid-jump animation)
        }
        
        direction.x = horizontal * speed;

        if (direction != new Vector3 (0, -1f, 0)) //only moves if player is not standing still (the -1 in y accounts for constant gravity)
        {
            characterController.Move(direction * Time.deltaTime); // called constantly (even which not moving) which allows the player to still jump even when still
        }
    

        if (direction.x != 0) //without the player would turn to face tiowards the backdrop when not receiving input as horizontal (wich drives direction.x) would be 0
        {
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, 0)); //y and z will stay zero 
            targetRotation *= Quaternion.Euler(0, turnDirectionMultiplier, 0); //makes Slerp below occur with the player turning facing towards the camera as opposed to towards the background which would happen by default (basically makes rotation on y axis negative so player turns in opposite direction to reach it)

            //smoothly rotates towards target point (might want to have this in its own function)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

    }

    void HandleJump() //handles jumping logic (in FixedUpdate)
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        isOnPlatform = Physics.CheckSphere(groundCheck.position, 0.2f, platformLayer);
        animator.SetBool("isGrounded", isGrounded || isOnPlatform);
        
        if (isGrounded || isOnPlatform)
        {
            
            direction.y = -1; //doesn't apply gravity when on the ground
            canDoubleJump = true; //resets ability to double jump after touching the ground
            if (initiatedJump) //if pressed space bar
            {
                animator.SetTrigger("jumped");
                Jump(jumpForce);
                initiatedJump = false; //resets spacebar press
            }
        }
        else //if not on the ground
        {
            direction.y += gravity * Time.deltaTime; //only applies gravity when not on the ground
            if (canDoubleJump && initiatedJump) //initiates doublejump if spacebar is pressed and candoublejump is true
            {
                animator.SetTrigger("doubleJump");
                DoubleJump();
                canDoubleJump = false;
                initiatedJump = false; //basically emulates when you release the spacebar
            }
        }

        if (isGrounded || !isOnPlatform)
        {
            transform.parent = null; //prevents bug in which player stays as child of a plaform even if not on it
        }


    }

    void CheckForJump() //initiates HandleJump (in Update)
    {
        if (!initiatedJump && canDoubleJump) //makes it so you can only trigger a jump if not currently jumping and if you have the ability to double jump
            initiatedJump = Input.GetButtonDown("Jump"); //makes should jump true upon button input
    }

    public void Jump(float force)
    { 
        playerAudio.PlayOneShot(jumpSound);
        direction.y = force;
    }

    public void DoubleJump()
    {
        playerAudio.PlayOneShot(doubleJumpSound);
        direction.y = jumpForce;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible && !playerHearts.IsDead())
        {
            playerAudio.PlayOneShot(hurtSound, 0.5f);
            playerHearts?.Damage(damage);

            if (hasFirePowerup)
            {
                playerAudio.PlayOneShot(losePowerupSound);
                hasFirePowerup = false; //makes it so fireball powerup goes away after getting hit
            }
        }
    }

    public void ReduceHearts(int damage)
    {
        playerHearts?.Damage(damage);
    }

    private void OnTriggerEnter(Collider other) 
    {

        /*if (other.CompareTag("Enemy") && isInvincible) //allows players to instakill enemies if they have the invicibility powerup
        {
            print("yee");
            other?.GetComponent<Enemy>().enemyHearts.Damage(invincibilityDamage);
        }*/

        if (other.CompareTag("Checkpoint"))
        {
            currentCheckpoint = other.transform;
        }
    }

    public void PowerupDuration(bool hasPowerup)
    {
        if (timeSincePowerup >= powerupDuration)
        {
            hasPowerup = false;
            timeSincePowerup = 0;
        }
        
        timeSincePowerup += Time.deltaTime;
    }

    private void CheckAndHandlePowerups()
    {
        /*if (isInvincible)
        {
            //speed *= 10f;
            if (timeSincePowerup >= powerupDuration)
            {
                isInvincible = false;
                //print(isInvincible);
                timeSincePowerup = 0;
            }

            timeSincePowerup += Time.deltaTime;
        }*/

        if (hasFirePowerup)
        {
            fireballUI.enabled = true;
            if (Input.GetKeyDown(KeyCode.F) && timeSinceFireball > fireballFireRate) //allows player to 
            {
                animator.SetTrigger("fireball");//animation event on the fireball animation for instantiating the fireball
            }
            //if (timeSincePowerup >= powerupDuration) //removes powerup after set time
            //{
            //    hasFirePowerup = false;
            //    timeSincePowerup = 0;
            //}
            timeSinceFireball += Time.deltaTime; //fireball firerate cooldown
        }
        else
        {
            fireballUI.enabled = false;
        }
    }

    public void FireFireball()
    {
        timeSinceFireball = 0;
        Vector3 pos = new Vector3 (castPosition.position.x, castPosition.position.y, 0); //technically don't need casterPos object anymore
        Quaternion rotation = Quaternion.Euler(0, 0, 0); //just initial value
        //spawns fireball and fires relative to right (x-axis) position of player
        if (transform.eulerAngles.y > 180)
        {
            pos.x = transform.position.x - 1.5f; //sets cast position to left of player if facing left
            rotation = Quaternion.Euler(0, 270f, 0);
            transform.rotation =  Quaternion.Euler(0, 270f, 0); //snaps player rotation to direction of firing the fireball
            turnDirectionMultiplier = 1; //makes player turn counter clockwise when turning to face the opposite direction
        }
        else
        {
            pos.x = transform.position.x + 1.5f; //sets cast position to right of player if facing right
            rotation = Quaternion.Euler(0, 90f, 0);
            transform.rotation = Quaternion.Euler(0, 90f, 0); //snaps player rotation to direction of firing the fireball
            turnDirectionMultiplier = -1; //makes player turn clockwise when turning to face the opposite direction
        }
        playerAudio.PlayOneShot(castSound);
        Instantiate(fireballProjectile, pos, rotation);
    }

    public void CheckIfMoving()
    {
        if (Mathf.Abs(horizontal) > 0 || (!isGrounded || isOnPlatform)) //exits animation early if moving horizontally or in the air
        {
            animator.SetTrigger("ExitAttack");
        }
    }

}
