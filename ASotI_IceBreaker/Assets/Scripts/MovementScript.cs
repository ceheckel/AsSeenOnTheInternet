using UnityEngine;

public class MovementScript : MonoBehaviour {

	// references
	public GameObject sm;
	public float speed;

	private bool isFinished; // determines if freighter crossed finish line


	//
	private void Start()
	{
		isFinished = false;
	}


	//
	private void Update()
	{
		
		// if boat is active ...
		if (gameObject.activeInHierarchy)
		{
			// ... push the boat forward
			gameObject.GetComponent<Rigidbody2D>().AddForce(
				gameObject.transform.up * speed * Time.deltaTime);

			// ... reset position if boat leaves play area
			if (gameObject.transform.position.y < -3)
			{
				gameObject.SetActive(false);
			}
			else if (gameObject.transform.position.y > 24)
			{
				// ... if the boat is an IB, destroy it
				if (gameObject.name.Contains("IB"))
				{
					gameObject.SetActive(false);
				}
				// ... if the boat is a Freighter
				else if (gameObject.name.Contains("FR"))
				{
					isFinished = true;
				}
			}
		}
	} // end of Update()


	//
	public bool IsFinished() { return isFinished; }
	public void SetFinished(bool fin) { isFinished = fin; }


	//
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// if freighter collides with something ...
		if (gameObject.name.Contains("FR") &&
			collision.collider.tag.Equals("Obstacle"))
		{
			// ... change sprite
			sm.GetComponent<BoatsScript>().ChangeFreighter(gameObject);

			// ... decrement score
			sm.GetComponent<StatsScript>().SetScoreValue(
				(int)sm.GetComponent<StatsScript>().GetScoreValue() - 100);
		}
		// if icebreaker collides with something ...
		else if (gameObject.name.Contains("IB") &&
			collision.collider.tag.Equals("Obstacle"))
		{
			// ... change ice sprite
			sm.GetComponent<TilesScript>().BreakSprite(collision.gameObject);
		}
	} // end of OnCollisionEnter2d()
}
