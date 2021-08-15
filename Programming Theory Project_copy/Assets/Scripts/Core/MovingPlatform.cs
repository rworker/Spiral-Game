using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 5f;

    private bool alreadyOn = false;
    private int currentWaypoint;


    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Count <= 0) return; //makes sure platform has waypoints before trying to move
        currentWaypoint = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waypoints.Count > 0) //avoids index out of range error in inspector
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position,
            (moveSpeed * Time.deltaTime));

        if (Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position) <= 0)
        {
            currentWaypoint++;
        }

        if (currentWaypoint != waypoints.Count) return; //has the function essentially keep looping in FixUpdate until the last waypoint is reached
        waypoints.Reverse(); //reverses order of waypoints to make platform go backwards
        currentWaypoint = 0; //the last waypoint is now the first waypoint
    }

    //private void OnTiggerEnter(Collider other) 
    //{
        //if (other.CompareTag("Player"))
        //{
        //    GameObject player = other.transform.parent.gameObject;
        //    player.transform.SetParent(gameObject.transform, true); //sets player (or enemy) as a child of the platform when colliding with it to make sure they move with the platform

        //}
        //print("true");
        //other.transform.SetParent(gameObject.transform, true);
    //}



    private void OnTriggerEnter(Collider other) 
    {
        if (!alreadyOn)
        {
            other.transform.SetParent(gameObject.transform, true); 
            alreadyOn = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.parent = null; //releases player/enemy as child when leaving the platform
        alreadyOn = false;
    }


}
