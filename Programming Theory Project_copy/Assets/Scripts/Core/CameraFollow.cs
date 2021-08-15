using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    private Vector3 cameraStartPos;
    private Vector3 offset;
    private bool followPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraStartPos = transform.position; //stores start position of camera
        offset = transform.position - player.position; //defines initial offset (the y pos here and when offset is redefined below should match)
        
    }

    // Update is called once per frame
    void Update()
    {
        //when player position equals the camera's start position, the camera begins following
        if (player.position.x >= cameraStartPos.x && !followPlayer) //adding !followplayer here prevents the offset from repeatedly being updated whenever player is to the right of the camerastartpos. Otherwise camera would not follow when moving past the camerastartpos and would only follow when the playerposition is less than the camerastartpos
        {
            offset = transform.position - player.position; //redefines offset upon passing 
            followPlayer = true;
        }
    }
    private void LateUpdate() 
    {
        if (followPlayer && player.position.x >= cameraStartPos.x) //if player reaches the camera's position the follow functionality is triggered. Prevents camera from following player when they move to the left of the Camerastartpos
        {
            transform.position = player.position + offset;
        }
        else //makes camera follow y position of player even when player is to the left of the camerastartpos (camera reacts to movement on y axis but not x)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);
        }
    }
        
}
