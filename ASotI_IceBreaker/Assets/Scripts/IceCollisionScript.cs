using UnityEngine;

public class IceCollisionScript : MonoBehaviour {

	public GameObject sm; // reference to the scene manager
	

	// invoked when the gameObject's collider is intersected by another collider
	// collision	- collider of the other object (not this object)
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// check to see if the other collider is from an Ice Breaker
		if (collision.gameObject.name.Contains("Boat") && 
			(!gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("wall")))
		{
			// ... if so, breakdown the sprite as necessary
			sm.GetComponent<LayTilesScript>().ChangeSprite(gameObject);

			// ... increment score stat
			sm.GetComponent<StatManagementScript>().IncrementCurrentScore(25);
		}

		// check if the current object has been broken down to "open water"
		if (gameObject.GetComponent<SpriteRenderer>().sprite.name.Equals("ow"))
		{
			// ... if so, deactivate the object so boats no longer collider with it
			gameObject.SetActive(false);
		}
	}
}
