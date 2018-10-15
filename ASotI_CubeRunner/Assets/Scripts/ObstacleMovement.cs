using UnityEngine;

public class ObstacleMovement : MonoBehaviour {

	// references
    public Transform player; // player object position
    public Transform obstacle; // obstacle position
    public Rigidbody obstacle_rigidbody; // obstacle's physics object
    public float chargeMagnitude; // amount of power to use when pushing obstacles
    
    private void FixedUpdate()
    {
        // check player proximity
        if (obstacle.position.x - player.position.x <= 15f
            && obstacle.position.z - player.position.z <= 50f)
        {
            // player is within pushing range of object
            // push object based on charge magnitude
            obstacle_rigidbody.AddForce(
                0,
                0,
                -1 * chargeMagnitude * Time.deltaTime,
                ForceMode.VelocityChange);
        }
    }
}
