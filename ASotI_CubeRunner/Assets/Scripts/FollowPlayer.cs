using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    // references
    public Transform player; // player position
    public Vector3 camera_position_offset; // camera offset from player
	
	// Update is called once per frame
	void Update () {
		// set camera position equal to player position with predetermined offset
        transform.position = player.position + camera_position_offset;
		// 'transform' refers to the object to which the script is attached
	}
}
