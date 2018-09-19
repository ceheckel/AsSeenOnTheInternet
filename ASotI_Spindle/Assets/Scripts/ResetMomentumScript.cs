using UnityEngine;

public class ResetMomentumScript : MonoBehaviour
{
	// references
	public float speed;

	private bool moveEnabled;

	private void Start()
	{
		moveEnabled = true;
	}

	// called once per frame for physics calculations
	private void FixedUpdate()
	{
		// game is playing and player is not bouncing ...
		if (moveEnabled)
		{
			if (gameObject.GetComponent<Rigidbody>().velocity.y == 0)
			{
				// ... give it a little boost
				gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, speed*50, 0));
			}
		}
		else
		{
			// stop ball
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (moveEnabled)
		{
			// Everytime the player's ball hits something, reset it's velocity
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, speed, 0);
		}
	}

	// setter for player movement enable boolean (inverse)
	internal void Suspend(bool val) { moveEnabled = !val; }
}
