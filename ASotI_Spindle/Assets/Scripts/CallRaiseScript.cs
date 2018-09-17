using UnityEngine;

public class CallRaiseScript : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		// access the script of the "Platforms" container to start raising platforms
		gameObject.transform.parent.parent.GetComponent<RaisePlatformScript>().IncRaiseValue();
	}
}
