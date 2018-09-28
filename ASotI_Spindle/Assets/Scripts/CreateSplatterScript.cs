using UnityEngine;

public class CreateSplatterScript : MonoBehaviour {

	// references
	public GameObject splatterTemplate;

	private void OnCollisionEnter(Collision collision)
	{
		// if the player's ball hits a platform ...
		if (collision.gameObject.name.Equals("Cylinder_LIVE"))
		{
			// ... create a splatter
			Instantiate
				(
					splatterTemplate,
					new Vector3(
						gameObject.transform.position.x,
						gameObject.transform.position.y - 0.4f,
						gameObject.transform.position.z + 1.0f
						),
					Quaternion.Euler(90.0f, 0.0f, Random.Range(0.0f, 360.0f)),
					collision.gameObject.transform.parent
				);

			// ... play audio clip

		}
	}
}
