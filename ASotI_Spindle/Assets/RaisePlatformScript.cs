using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisePlatformScript : MonoBehaviour {

	public float speedScale;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Triggered");

		StartCoroutine("Raise");
	}

	private void Raise() {
		gameObject.transform.parent.transform.Translate(Vector3.up * speedScale * Time.deltaTime);

		if (transform.position.y >= 0)
		{
			StopCoroutine("Raise");
		}
	}
}
