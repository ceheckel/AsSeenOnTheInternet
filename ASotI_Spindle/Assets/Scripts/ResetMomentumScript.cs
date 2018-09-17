using UnityEngine;

public class ResetMomentumScript : MonoBehaviour
{
	// references
	public float speed;

	private void OnCollisionEnter(Collision collision)
	{
		// Everytime the player's ball hits something, reset it's velocity
		Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
		v = v.normalized;
		v *= speed;
		gameObject.GetComponent<Rigidbody>().velocity = v;
	}
}
