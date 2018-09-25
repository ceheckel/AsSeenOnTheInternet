using UnityEngine;

public class RaisePlatformScript : MonoBehaviour
{
	// references 
	private Rigidbody prb; // player's rigidbody component
	private GameObject gc; // GameController object
	private Vector3 target; // target locations of each platform after raise
	private int needsRaise; // number of times the platforms need to be raised

	private void Awake()
	{
		gc = GameObject.Find("GameController");
		if (gc == null) { Debug.LogWarning("RaisePlatformScript: gc not found"); }

		prb = GameObject.Find("Player").GetComponent<Rigidbody>();
		if (prb == null) { Debug.LogWarning("RaisePlatformScript: prb not found"); }

		target = new Vector3(0, 5, 0);
		needsRaise = 0;
	}

	// increments the counter that corresponds to the number of tiers the
	// platforms must be raised
	internal void IncRaiseValue()
	{
		// Begin platform raising process
		needsRaise += 1;
	}

	// getter for the needsRaise variable
	// used before lowering the end game screen to prevent the ball from
	// spawning early
	internal int GetRaiseValue() { return needsRaise; }

	// called once per frame
	private void Update()
	{
		if (needsRaise > 0)
		{
			foreach (GameObject p in gc.GetComponent<PlatformManagementScript>().platforms)
			{
				if (p != null)
				{
					// move platform up
					p.transform.position = Vector3.MoveTowards
						(
							p.transform.position,
							target,
							Time.deltaTime * 3
						);

					// when a platform reaches the top ...
					if (p.transform.position.y >= 5)
					{
						// ... Stop raising platforms
						needsRaise -= 1;

						// ... Destroy platform, p
						gc.GetComponent<PlatformManagementScript>().RemovePlatform();

						// ... Create new platform
						gc.GetComponent<PlatformManagementScript>().AddPlatform();
					}
				}
			}
		}
	}
}