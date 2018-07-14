using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    // reference to player position
    public Transform player;
    public Vector3 camera_position_offset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Player is at location: " + player.position);

        // 'transform' refers to the object that the script is attached to
        transform.position = player.position + camera_position_offset;
	}
}
