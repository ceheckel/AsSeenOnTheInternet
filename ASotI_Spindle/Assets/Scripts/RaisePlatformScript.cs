using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisePlatformScript : MonoBehaviour
{

	public float speedScale;

	private GameObject gc;
	private Vector3 target;
	private bool needsRaise;

	private void Awake()
	{
		gc = GameObject.Find("GameController");
		if (gc == null) { Debug.LogWarning("RaisePlatformScript: gc not found"); }

		target = new Vector3(0, 5, 0);
		needsRaise = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		// Begin platform raising process
		needsRaise = true;
	}

	private void Update()
	{
		if (needsRaise)
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
							Time.deltaTime * speedScale
						);

					// when a platform reaches the top ...
					if (p.transform.position.y >= 5)
					{
						// ... Stop raising platforms
						needsRaise = false;

						// ... Destroy platform, p
						Debug.Log("Dead");
						gc.GetComponent<PlatformManagementScript>().RemovePlatform();

						// ... Create new platform
						Debug.Log("Create");
						gc.GetComponent<PlatformManagementScript>().AddPlatform();
					}
				}
			}
		}
	}
}