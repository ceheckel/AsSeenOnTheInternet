using UnityEngine;

public class EndTrigger : MonoBehaviour {

	// references
    public GameManager gameManager;

	// called when an object collides with the object to which this script is attached
    void OnTriggerEnter()
    {
		// assuming that the player object will be the only thing moving,
		// no safety check is needed

		// raise level ending screen
        gameManager.CompleteLevel();
    }
}
