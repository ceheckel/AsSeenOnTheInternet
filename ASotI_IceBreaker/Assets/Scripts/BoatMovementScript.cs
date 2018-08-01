using UnityEngine;

public class BoatMovementScript : MonoBehaviour {

	public GameObject sm;
	public float forwardSpeed;

	private bool finishState;


	// Use this for initialization
	private void Start()
	{
		finishState = false;
	}


	// Update is called once per frame
	void Update () {

		// if the boat is active ...
		if (gameObject.activeInHierarchy)
		{
			// ... push the boat in the direction it is facing
			gameObject.GetComponent<Rigidbody2D>().AddForce(
				gameObject.transform.up * forwardSpeed);

			// ... check to see if boat left through bottom gap
			if (gameObject.transform.position.y < -3) { DestroyBoat(); }
			// ... check to see if boat left through top gap
			else if (gameObject.transform.position.y > 24)
			{
				// ... check to see which type of boat left
				if (gameObject.name.Contains("Boat"))
				{
					// ... destroy boat if it was an IB
					DestroyBoat();
				}
				else if (gameObject.name.Contains("Freighter"))
				{
					// ... mark as finished if boat was a freighter
					finishState = true;
					//gameObject.SetActive(false);

					Debug.Log("freighter status: " + finishState);
				}
			}
		}
	} // end of Update()


	// sets the current gameObject to inactive in hierarchy
	private void DestroyBoat()
	{
		gameObject.SetActive(false);
	}

	public bool IsFinished() { return finishState; }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if freighter collides will something ...
		if (gameObject.name.Contains("Freighter") &&
			collision.collider.tag.Equals("Obstacle"))
		{
			// ... change sprite
			sm.GetComponent<BoatManagementScript>().ChangeFreighterSprite(gameObject);

			// ... decrement score
			sm.GetComponent<StatManagementScript>().IncrementCurrentScore(-100);
		}
	} // end of OnCollisionEnter2d()
}
