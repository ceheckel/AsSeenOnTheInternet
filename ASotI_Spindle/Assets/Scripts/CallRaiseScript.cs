using UnityEngine;

public class CallRaiseScript : MonoBehaviour {

	private GameObject gc;

	private void Start()
	{
		gc = GameObject.Find("GameController");
		if (gc == null) { Debug.LogWarning("CallRaiseScript: GameController not found"); }
	}

	private void OnTriggerEnter(Collider other)
	{
		// if the game is in session, listen to trigger collision
		if (gc.GetComponent<GameManagementScript>().gameOn)
		{
			// access the script of the "Platforms" container to start raising platforms
			gameObject.transform.parent.parent.GetComponent<RaisePlatformScript>().IncRaiseValue();

			// access the gamecontroller and increment the score
			gc.GetComponent<GameManagementScript>().SetScore(
				gc.GetComponent<GameManagementScript>().GetScore() + 1);
		}
	}
}
