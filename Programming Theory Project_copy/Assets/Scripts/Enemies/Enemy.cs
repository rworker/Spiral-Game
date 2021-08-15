using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public CharacterController enemyController;
    private ScoreTracker playerScore;
    [SerializeField] private float roamSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float maxDistanceFromStartPos;
    [SerializeField] private float chaseRange; //range in which enemy will begin chasing
    [SerializeField] private float attackRange; //range in which enemy will attack
    [SerializeField] private float returnRange; //range in which enemy will give up on chase and return
    [SerializeField] private float turnSpeed;
    [SerializeField] private float idleTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform obstacleCheckTransform;
    [SerializeField] private LayerMask obstacleLayer;
    public LayerMask groundLayer; //so it can be accessed in the jumping enemy class

    public enum State
    {
        Roaming,
        ChaseTarget,
        Attack,
        Return,
        Dying
    }

    private Vector3 startingPos;
    protected Vector3 direction;
    private Vector3 destination;
    public State currentState;
    private float timeSinceArrived = 0;
    private float timeSinceAttacked = 0;
    private bool canChooseNewDestination = true;
    private bool isIdle = false;
    protected bool isAttacking = false; //used to stop enemy movement during attacking
    //private bool alreadyWaiting = false;
    //private bool encounteredObstacle = false;
    private bool isActive = false;
    private bool canAttack = true;
    private bool deathAlreadyTriggered = false;
    
    private float stoppingDistance;

    public HeartsSystem enemyHearts;
    public int numberOfHearts = 1;
    public int scoreWorth = 10;

    protected PlayerController player;
    public Collider enemyCollider;

    protected AudioSource enemyAudio;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    public event EventHandler OnDead;

    [SerializeField] protected Animator animator; //so can access in subclass

    private void Awake() 
    {
        enemyHearts = new HeartsSystem(numberOfHearts);
        currentState = State.Roaming;
        enemyCollider = GetComponent<Collider>();
        enemyAudio = GetComponent<AudioSource>();
    }

    public virtual void Start() 
    {
        startingPos = transform.position;
        playerScore = FindObjectOfType<ScoreTracker>();
        player = FindObjectOfType<PlayerController>();
        this.OnDead += Enemy_OnDead; //subscribes function
    }

    private void Update() 
    {
        if (enemyHearts.IsDead())
        {
            currentState = State.Dying;
        }

        if (Vector3.Distance(player.transform.position, transform.position) < 40f)
        {
            isActive = true;
        }
    }

    public virtual void FixedUpdate() 
    {
        if (isActive) //ensures statemachine is only running if player is within given range
        {
            switch (currentState)
            {
                default:
                case State.Roaming:
                    if (canChooseNewDestination) //if has reached destination and waited, look for new desination
                    {
                        destination = GetRandomDestination(); //gets a random destination within a range determined by starting position and maxdistancefromstartpos
                        direction = GetDirection(transform.position, destination); //determines the direction in order to get to the destination from the current position
                        canChooseNewDestination = false;
                    }
                    if (!isIdle)
                    {
                        RotateTowardsDestination();
                        Move(direction, roamSpeed); //moves towards destination if not idling
                        animator.SetFloat("Speed", roamSpeed);
                    }
                    FindTarget(); //searches for target and switches to chase state if they are spotted

                    if (HasReachedDestination(destination) && destination != startingPos)
                    {
                        isIdle = true; //stops move function
                        WaitThenMove(idleTime);
                    }
                    break;
                case State.ChaseTarget:
                    canAttack = true; //makes it so enemy can always immediately attack upon coming within range of player
                    destination = player.transform.position;
                    direction = GetDirection(transform.position, destination);
                    if (player.transform.position.x > transform.position.x - 0.2 && player.transform.position.x < transform.position.x + 0.2) //makes enemy go idle if player is above them
                    {
                        isIdle = true; //stops move function
                    }
                    else
                    {
                        isIdle = false;
                    }

                    RotateTowardsDestination(); //makes sure enemy fully rotates towards player even idling (enemy won't half-rotate if player is above them)
                    if (!isIdle)
                    {
                        Move(direction, chaseSpeed);
                        animator.SetFloat("Speed", chaseSpeed);
                    }

                    CheckIfInAttackRange(); //checks if can attack
                    CheckIfOutsideChaseRange(); //logic for if player moves too far away to switch state to return to starting pos
                    break;

                case State.Attack:
                    isIdle = true; //makes animation go back to idle in between attacks when in attacking range
                    destination = player.transform.position;
                    RotateTowardsDestination(); //makes enemy always look towards player even when attacking
                    if (canAttack)
                    {
                        isAttacking = true;
                        
                        AttackAnim(); //plays attack animation which has an animation event to perform the actual attack (Attack() method(aka cast projectile or deal damage)
                        canAttack = false;
                    }
                    if (!isAttacking) //only allows to return to chase state if finished attack (may not mean they necessarily finished the animation)
                    {
                        CheckIfOutsideAttackRange(); //checks if should return to chase state and if so triggers enemy to exit attack animation and move
                    }
                    break;
                case State.Return:
                    if (isIdle) //waits at start of return state
                    {
                        WaitThenMove(idleTime);
                    }
                    if (canChooseNewDestination)
                    {
                        destination = startingPos;
                        direction = GetDirection(transform.position, destination);
                        canChooseNewDestination = false;
                    }
                    
                    if (!isIdle) //if not idle
                    {
                        RotateTowardsDestination();
                        Move(direction, roamSpeed);
                        animator.SetFloat("Speed", roamSpeed);
                    }
                    FindTarget(); //continues to search for target when returning
                    if (HasReachedDestination(destination)) //checks if reached starting position
                    {
                        isIdle = true;
                        WaitThenMove(idleTime);
                        if (canChooseNewDestination)
                        {
                            currentState = State.Roaming; //changes state to roaming if starting position is reached
                        }
                    }
                    break;

                case State.Dying:
                    isIdle = true; //redundant since there isn't movement in this script anyway
                    if (OnDead != null) OnDead(this, EventArgs.Empty); //triggers Ondead event which disables colliders

                    
                    if (!deathAlreadyTriggered) //prevents the animation running repeatedly or player constantly getting score
                    {
                        enemyAudio.PlayOneShot(deathSound);
                        animator.SetTrigger("isDead"); //plays death animation which also has an animation event to call the Die() function which destroys the game object
                        playerScore.AddtoScore(scoreWorth);
                        deathAlreadyTriggered = true;
                    }

                    break;
            }   
        }

        if (isIdle)
        {
            animator.SetFloat("Speed", 0); //sets idle animation whenever isIdle
        }

        if (!canAttack) //checks if recently attacked (code located outside of attack state so the cooldown will occur regardless of the state currently in)
        {
            StartAttackCooldown(); //timer which eventually allows enemy to attack again
        }

        transform.position = new Vector3 (transform.position.x, startingPos.y, 0); //ensure enemy does not move it's z position
    }

    private Vector3 GetDirection(Vector3 currentPos, Vector3 destination)
    {
        float xDirection = destination.x - currentPos.x;
        return new Vector3(xDirection, 0, 0).normalized;
    }

    public virtual void Move(Vector3 direction, float speed)
    {
        
        //transform.LookAt(new Vector3 (destination.x, transform.position.y, transform.position.z));

        if (!SensedObstacle()) //only moves if there is not a wall/obstacle blocking the player
        {
            enemyController.Move(direction * Time.deltaTime * speed);
        }
        else
        {
            isIdle = true; //stops movement
            canChooseNewDestination = false;
            currentState = State.Return; //makes enemy return back to start after hitting an obstacle
        }
    }

    private void RotateTowardsDestination()
    {
        var targetRotation = Quaternion.LookRotation(new Vector3(destination.x, transform.position.y, transform.position.z) - transform.position, Vector3.up);

        //smoothly rotates towards target point (might want to have this in its own function)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void FindTarget()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) < chaseRange)
        {
            currentState = State.ChaseTarget;
        }
    }

    private Vector3 GetRandomDestination()
    {

        float randomX = UnityEngine.Random.Range(startingPos.x + maxDistanceFromStartPos, startingPos.x - maxDistanceFromStartPos); //gets random x value within the positive and negative max range from the startingPos
        //the above is restrictive as if the enemy is already near the upper bounds it won't be able to move much further and will look weird
        //float exclusionValue = 6;
        //float randomXAdj = randomX >= 0 ? (randomX + exclusionValue) : (randomX - exclusionValue); //ensures randomX position excludes values between -exclusionvalue and exclusionvalue
        while (Mathf.Abs(transform.position.x - randomX) < 6f) //keeps looking for a destination until it is at least 6 away from the player
        {
            //this probably isn't the most efficient way but it'll work
            randomX = UnityEngine.Random.Range(startingPos.x + maxDistanceFromStartPos, startingPos.x - maxDistanceFromStartPos); //had to add UnityEngine. to make call unambiguous (System also has a "Random" call)
        }
        return new Vector3(randomX, 0, 0);
    }

    private bool HasReachedDestination(Vector3 destination)
    {
        return (direction.x < 0 && transform.position.x <= destination.x) || (direction.x > 0 && transform.position.x >= destination.x);
    }

    private void CheckIfInAttackRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < attackRange)
        {
            currentState = State.Attack;
        }
    }

    private void CheckIfOutsideAttackRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > attackRange)
        {
            //if (!isIdle) //checks if enemy is not idle meaning, by default given it is currently in the attack state, it would be attacking

            animator.SetTrigger("ExitAttack"); //make enemy exit its attack animation (assuming it is attacking and not in between attacks aka idle) changes it's state to chase

            currentState = State.ChaseTarget;
        }
    }

    private void CheckIfOutsideChaseRange()
    {
        if (Mathf.Abs((player.transform.position.x - transform.position.x)) > returnRange)
        {
            isIdle = true; //stops move function
            canChooseNewDestination = false; //makes it so player can't choose new destination until done idling
            currentState = State.Return;
        }
    }

    public virtual void Attack()
    {
        enemyAudio.PlayOneShot(attackSound);
    }

    public virtual void AttackAnim()
    {
        animator.ResetTrigger("ExitAttack");
        animator.SetTrigger("Attack");
    }

    public virtual void ApplyModifiers()
    {
        //insert jumping code
    }

    private bool SensedObstacle()
    {
        Vector3 offset = new Vector3(0, 1.2f, 0);
        Vector3 startPnt = new Vector3(obstacleCheckTransform.position.x, startingPos.y, startingPos.z) + offset; //accounts for jumping enemies
        Vector3 endPnt = new Vector3(obstacleCheckTransform.position.x, startingPos.y, startingPos.z) - offset;
        bool obstacleCheck = Physics.CheckCapsule(startPnt, endPnt, 0.5f, obstacleLayer);
        bool groundCheck = Physics.CheckCapsule(startPnt, endPnt, 0.5f, groundLayer);

        if (!groundCheck)
        {
            return true;
        }
        if (obstacleCheck)
        {
            return true;
        }

        return false; //returns false by default
    }

    private void WaitThenMove(float idleTime)
    {
        //alreadyWaiting = true;

        if (timeSinceArrived >= idleTime) //checks if has waited long enough
        {
            isIdle = false; //re-enables movement
            canChooseNewDestination = true; //re-enables enemy's ability to choose new destination
            timeSinceArrived = 0;//resets time since arrived for next idle period
        }

        timeSinceArrived += Time.deltaTime; //timer for waiting
    }

    private void StartAttackCooldown()
    {
        if (timeSinceAttacked >= attackCooldown)
        {
            canAttack = true;
            timeSinceAttacked = 0;
        }

        timeSinceAttacked += Time.deltaTime;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void DoneAttacking() //used to serve as function for animation event, changes isAttacking to false and allows enemy the option to move again (if it needs to chase player)
    {
        isAttacking = false;
    }

    private void Enemy_OnDead(object sender, System.EventArgs e)
    {
        enemyCollider.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        enemyAudio.PlayOneShot(hurtSound);
        enemyHearts.Damage(damage);
    }

}
