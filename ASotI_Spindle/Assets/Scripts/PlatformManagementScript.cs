using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagementScript : MonoBehaviour {

	// references
	public float speedScale;
	public GameObject ball;
	public GameObject[] platTypes;

	private GameObject[] platforms;
	private int index; // index counter for next open spot in platforms array
	private readonly int numPlats = 10;
	private static PlatformManagementScript instance;

	private void Awake()
	{
		if (instance != null && instance != this) { Destroy(this.gameObject); }
		else { instance = this;	}
	}

	// Use this for initialization
	void Start () {
		platforms = new GameObject[numPlats];
		index = 0;

		FindPlatforms();
	}

	private void FindPlatforms()
	{
		platforms = GameObject.FindGameObjectsWithTag("Platform");
		if (platforms == null || platforms.Length == 0)
		{
			Debug.LogWarning("FindPlatforms: No Platforms were found");
			return;
		}

		index = platforms.Length;
	}

	private void AddPlatform()
	{
		// create new object at the bottom of the post
		platforms[index] = Instantiate
			(
				RandomPlatform(),
				new Vector3
				(
					platforms[PrevEmpty()].transform.position.x,
					platforms[PrevEmpty()].transform.position.y - 5,
					platforms[PrevEmpty()].transform.position.z
				),
				platforms[PrevEmpty()].transform.rotation,
				platforms[PrevEmpty()].transform.parent
			);
		
		// manipulate new platform


		// manage counter
		IncIndex();
	}

	private void RemovePlatform()
	{
		DecIndex();

		Destroy(platforms[index]);
		platforms[index] = null;
	}

	private GameObject RandomPlatform()
	{
		return platTypes[Random.Range(0, platTypes.Length)];
	}

	private int PrevEmpty() { return (index == 0 ? numPlats - 1 : index - 1); }
	private int NextEmpty() { return (index + 1) % numPlats; }
	private void IncIndex() { index = (index + 1) % numPlats; }
	private void DecIndex() { index = (index == 0 ? numPlats - 1 : index - 1); }
}
