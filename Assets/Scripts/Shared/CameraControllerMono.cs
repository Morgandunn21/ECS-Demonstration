using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMono : MonoBehaviour
{
    /// <summary>
    /// Player object that the camera follows
    /// </summary>
    public GameObject player;

    /// <summary>
    /// How smoothly the camera follows
    /// </summary>
    public float easing = 0.5f;

    /// <summary>
    /// How high the camera is
    /// </summary>
    private Vector3 cameraOffset;

    private void Awake()
    {
        cameraOffset = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Where the camera is headed this update
        Vector3 destination;

        //if there is no player
        if (player == null)
        {
            //move to (0,0,0)
            destination = Vector3.zero;
        }
        else    //If there is a player
        {
            //Set destination to where the player is
            destination = player.transform.position;
            destination.z += cameraOffset.z;
        }
        //Move between the two points using the easing to smooth motion
        destination = Vector3.Lerp(transform.position, destination, easing);
        //keep y value the same
        destination.y = cameraOffset.y;
        //move camera to the calculated position
        transform.position = destination;
    }
}
