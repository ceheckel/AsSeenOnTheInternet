using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagementScript : MonoBehaviour {

	// references
	public GameObject ball;
	public GameObject[] platTypes;

	internal GameObject[] platforms;
	private int index; // index counter for next open spot in platforms array
	private readonly int numPlats = 4;
	private static PlatformManagementScript instance;

	private void Awake()
	{
		// create singleton
		if (instance != null && instance != this) { Destroy(this.gameObject); }
		else { instance = this;	}
	}

	// Use this for initialization
	void Start () {
		platforms = new GameObject[numPlats];
		index = 0;

		Debug.Log("index0: " + index);

		int numFound = FindPlatforms();

		Debug.Log("index1: " + index);
		Debug.Log("found1: " + numFound);

		if (numFound < numPlats)
		{
			for (int i = numFound; i < numPlats; i += 1)
			{
				AddPlatform();
			}
		}
	}

	private int FindPlatforms()
	{
		// find all existing platforms in the scene
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Platform");

		// if none were found ...
		if (temp == null || temp.Length == 0)
		{
			// ... notify the boss
			Debug.LogWarning("FindPlatforms: No Platforms were found");
			return 0;
		}

		// Since FindGameObjectsWithTag() returns a new array of the perfect size
		// we need to copy the search results into our management array.
		for (int i = 0; i < temp.Length; i += 1)
		{
			platforms[i] = temp[i];
		}

		// mark the next available index
		index = temp.Length;
		return index;
	}

	internal void AddPlatform()
	{
		Vector3 pos;
		Quaternion rot;
		Transform par = GameObject.Find("Platforms").transform;

		// get platform template
		if (platforms[PrevIndex()] == null)
		{

			Debug.Log("index2: " + index);

			pos = new Vector3(0, -5, 0);
			rot = new Quaternion();
		}
		else
		{
			pos = new Vector3(0, platforms[PrevIndex()].transform.position.y - 5, 0);
			rot = platforms[PrevIndex()].transform.rotation;

			Debug.Log("index3: " + index);

		}

		// create new object at the bottom of the post
		platforms[index] = Instantiate(RandomPlatform(), pos, rot, par);

		// manage counter
		IncIndex();

		Debug.Log("index4: " + index);

	}

	internal void RemovePlatform()
	{
		Destroy(platforms[index]);
		platforms[index] = null;
	}

	private GameObject RandomPlatform()
	{
		return platTypes[Random.Range(0, platTypes.Length)];
	}

	private int PrevIndex() { return (index == 0 ? numPlats - 1 : index - 1); }
	private int NextIndex() { return (index + 1) % numPlats; }
	private void IncIndex() { index = (index + 1) % numPlats; }
	private void DecIndex() { index = (index == 0 ? numPlats - 1 : index - 1); }
}
