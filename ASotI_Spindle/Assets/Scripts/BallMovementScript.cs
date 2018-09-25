using UnityEngine;

public class BallMovementScript : MonoBehaviour
{
	// references
	public float speed;

	private bool moveEnabled;
	private int direction; // either 1 or -1 for vertical direction

	private void Start()
	{
		moveEnabled = true;
		direction = -1;
	}

	// called once per frame for physics calculations
	private void FixedUpdate()
	{
		// game is playing and player is not bouncing ...
		if (moveEnabled)
		{
			gameObject.transform.Translate(Vector3.up * direction * speed * Time.deltaTime);
		}

		if (gameObject.transform.position.y >= 3) { direction = -1; }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (moveEnabled)
		{
			direction = 1;
		}
	}

	// setter for player movement enable boolean (inverse)
	internal void Suspend(bool val) { moveEnabled = !val; }

	private void SpeedUp()
	{
		float ypos = gameObject.transform.position.y;

		if (3 - ypos >= 0.5)
		{
			// move ball up or down
			gameObject.transform.Translate(Vector3.up * direction * speed * Time.deltaTime);
		}
		else
		{
			// add a little arch at the top of the bounce
			gameObject.GetComponent<Rigidbody>().AddForce
				(Vector3.up * direction * speed * 5 * Time.deltaTime);
		}

		if (ypos >= 4) { direction = -1; }
	}
}
