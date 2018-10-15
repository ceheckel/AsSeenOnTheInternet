using UnityEngine;

public class PlatformManagementScript : MonoBehaviour {

	// references
	public GameObject ball;
	public GameObject[] platTypes;

	internal GameObject[] platforms;

	private int index; // index counter for next open spot in platforms array
	private readonly int numPlats = 5;
	private static PlatformManagementScript instance;

	// checks to see if this gameObject is the only one with the script,
	// if so, it becomes a singleton instance, else it destroys itself to prevent
	// double functionality
	private void Awake()
	{
		// create singleton
		if (instance != null && instance != this) { Destroy(this.gameObject); }
		else { instance = this;	}
	}

	// Use this for initialization
	// if no platforms exist at the beginning of the game,
	// this function creates them and fills the tracking array
	void Start () {
		platforms = new GameObject[numPlats];
		index = 0;
		
		int numFound = FindPlatforms();
		if (numFound < numPlats)
		{
			for (int i = numFound; i < numPlats; i += 1)
			{
				AddPlatform();
			}
		}
	}

	// finds all existing platforms (if any) when the game starts
	// return value is the next empty index of the tracking array,
	// if the tracking array is full, return value is the oldest platform
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

	// adds a new platform to the hierarchy and tracking array
	internal void AddPlatform()
	{
		// info for new platform
		Vector3 pos;
		Quaternion rot = new Quaternion(0, Random.Range(0, 359), 0, 0);
		Transform par = GameObject.Find("Platforms").transform;

		// safety check the found object
		if (par == null) { Debug.LogWarning("AddPlatforms: Parent platform object was not found"); }

		// get platform template
		if (platforms[PrevIndex()] == null)
		{
			pos = new Vector3(0, -5, 0);
		}
		else
		{
			pos = new Vector3(0, platforms[PrevIndex()].transform.position.y - 5, 0);
		}

		// create new object at the bottom of the post
		platforms[index] = Instantiate(RandomPlatform(), pos, rot, par);

		// manage counter
		IncIndex();
	}

	// remove oldest platform from hierarchy and tracking array
	internal void RemovePlatform()
	{
		Destroy(platforms[index]);
		platforms[index] = null;
	}

	// returns a GameObject with the properties of a random platform style
	private GameObject RandomPlatform()
	{
		return platTypes[Random.Range(0, platTypes.Length)];
	}

	// returns the previous index of the tracking array (round-robin style)
	private int PrevIndex() { return (index == 0 ? numPlats - 1 : index - 1); }
	// returns the next index of the tracking array (round-robin style)
	private int NextIndex() { return (index + 1) % numPlats; }
	// increments the index counter for the platform tracking array (round robin style)
	private void IncIndex() { index = (index + 1) % numPlats; }
	// decrements the index counter for the platform tracking array (round robin style)
	private void DecIndex() { index = (index == 0 ? numPlats - 1 : index - 1); }
}
