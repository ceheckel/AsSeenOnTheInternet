using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    // reference to player movement script
    public PlayerMovement movement;
    
    // logic for collision between two objects
    void OnCollisionEnter(Collision collisionInfo)
    {
        // adding a tag to an object and checking its tag for comparison is 
        // much better than comparing to the collider's name.
        // this allows us to have scalable amounts of objects of the same type
        // with the same logic
        if (collisionInfo.collider.tag == "Obstacle")
        {
            // turn off movement when an obstacle is hit
            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame(); // returns an error if zero GameManager Objects exist

            // Play death note on collision with obstacle
            // Theme song stops for better experience, then restarts afterwards
            FindObjectOfType<AudioManager>().StopSong("Theme");
            FindObjectOfType<AudioManager>().PlaySong("PlayerDeath");
            //FindObjectOfType<AudioManager>().PlaySong("Theme");
        }
    }
}
