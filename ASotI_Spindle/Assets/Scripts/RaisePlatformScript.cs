using UnityEngine;

public class RaisePlatformScript : MonoBehaviour
{
	// references 
	private Rigidbody prb;
	private GameObject gc;
	private Vector3 target;
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

	internal void IncRaiseValue()
	{
		// Begin platform raising process
		needsRaise += 1;
	}

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

						Debug.Log(needsRaise);

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