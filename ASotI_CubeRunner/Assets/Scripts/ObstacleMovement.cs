using UnityEngine;

public class ObstacleMovement : MonoBehaviour {

    public Transform player;
    public Transform obstacle;
    public Rigidbody obstacle_rigidbody;
    public float chargeMagnitude;
    
    private void FixedUpdate()
    {
        //Debug.Log("Player position: <" + player.position.x + ", " + player.position.z + ">");
        //Debug.Log("Obstacle position: <" + obstacle.position.x + ", " + obstacle.position.z + ">");
        //Debug.Log("Delta position: <" + (player.position.x + obstacle.position.x) + ", " + 
        //    (player.position.z + obstacle.position.z) + ">");

        // check player proximity
        if (obstacle.position.x - player.position.x <= 15f
            && obstacle.position.z - player.position.z <= 50f)
        {
            // player is within pushing range of object
            Debug.Log("Player near Charging Obstacle");

            // push object based on charge magnitude
            obstacle_rigidbody.AddForce(
                0,
                0,
                -1 * chargeMagnitude * Time.deltaTime,
                ForceMode.VelocityChange);
        } else
        {
            obstacle_rigidbody.AddForce(0, 0, 0);
        }
    }
}
