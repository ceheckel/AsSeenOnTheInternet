using UnityEngine;

public class CallDeathScript : MonoBehaviour {

	private void OnCollisionEnter(Collision collision)
	{
		// check if player hit the dead zone
		if (collision.collider.name.Equals("Player"))
		{
			// play absorption animation


			// bring up credits
			GameObject.Find("GameController").GetComponent<GameManagementScript>().RaiseEndGame();
		}
	}
}