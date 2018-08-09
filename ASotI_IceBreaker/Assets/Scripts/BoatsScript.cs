using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatsScript : MonoBehaviour {

	// references
	public float launchDelay;
	public float defaultTime;
	public int numIBs;
	public int numFrs;
	public GameObject frTemp;
	public GameObject ibTemp;
	public Sprite[] frSprites;

	private List<GameObject> freighters;
	private List<GameObject> iceBreakers;
	private bool boatsCloned = false;
	private int launchNumber;
	private float restartTimer;


	//
	private void Start()
	{
		freighters = new List<GameObject>();
		iceBreakers = new List<GameObject>();

		// start at launch stage zero
		launchNumber = 0;

		restartTimer = defaultTime;
	}


	// Update is called once per frame
	void Update()
	{
		// when user gives the word, launching IBs
		if (launchNumber == 1)
		{
			StartCoroutine(LaunchBoatsRepeat(iceBreakers, launchDelay));

			// prevent relaunch of IBs
			launchNumber = 2;
		}
		else if (launchNumber == 2)
		{
			restartTimer -= Time.deltaTime;
			if (restartTimer <= 0) { launchNumber = 3; }
		}
		// when user gives the word, launching FRs
		else if (launchNumber == 4)
		{
			StartCoroutine(LaunchBoatsRepeat(freighters, launchDelay));

			// prevent relaunch of IBs
			launchNumber = 5;
		}
	}


	//
	private void FixedUpdate()
	{
		// check for start signal and active scene
		if (Input.anyKey && launchNumber == 0 &&
			SceneManager.GetActiveScene().name.Equals("LevelScene")) { launchNumber = 1; }
		if (Input.anyKey && launchNumber == 3) { launchNumber = 4; }
	}


	// 
	internal void CreateBoats()
	{
		// only clone boats once
		if (boatsCloned) { return; }

		// clone the freighter "numFrs" number of times
		for (int i = 0; i < numFrs; i += 1)
		{
			// clone the freighter
			GameObject obj = Instantiate(frTemp);

			// set properties
			obj.name = "Freighter [" + i + "]";
			obj.transform.parent = frTemp.transform.parent;
			obj.SetActive(false);

			// add to tracking list
			freighters.Add(obj);
		}

		// clone the ice breakers "numIBs" number of times
		for (int i = 0; i < numIBs; i += 1)
		{
			// clone the IB
			GameObject obj = Instantiate(ibTemp);

			// set properties
			obj.name = "IB [" + i + "]";
			obj.transform.parent = ibTemp.transform.parent;
			obj.SetActive(false);

			// add to tracking list
			iceBreakers.Add(obj);
		}

		boatsCloned = true;
	} // end of CreateBoats()


	//
	internal void ChangeFreighter(GameObject fr)
	{
		for (int i = 0; i < frSprites.Length-1; i += 1)
		{
			if (fr.GetComponent<SpriteRenderer>().sprite == frSprites[i])
			{
				fr.GetComponent<SpriteRenderer>().sprite = frSprites[i + 1];
				
				// prevent ship from sinking after one hit
				return;
			}
		}

		// if sprite not found, freighter has no health
		// play sinking animation
		Debug.Log("Sinking");

		// move to credits
		Invoke("EndGame", 2f);
	}


	//
	private void EndGame()
	{
		gameObject.GetComponent<SceneManagerScript>().LoadLevel(3);
	}


	//
	internal void SetBoatsActive(bool val)
	{
		// get all game objects
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Boat");
		// iterate through them
		foreach (GameObject o in obj)
		{
			// set activity value
			o.SetActive(val);
		}
	}


	// wrapper method used instead of InvokeRepeating due to parameter needs
	// This technique is a workaround
	IEnumerator LaunchBoatsRepeat(List<GameObject> boats, float launchDelay)
	{
		while (true)
		{
			LaunchBoats(boats);
			yield return new WaitForSeconds(launchDelay);
		}
	}


	//
	internal void LaunchBoats(List<GameObject> boats)
	{
		// for each boat in the provided list ...
		foreach (GameObject b in boats)
		{
			// ... find a boat that is disabled and unfinished ...
			if ((!b.activeInHierarchy) &&
				(!b.GetComponent<MovementScript>().IsFinished()))
			{
				// ... move it to the launch position
				b.transform.SetPositionAndRotation(
					GetComponent<ArrowScript>().arrow.transform.position,
					GetComponent<ArrowScript>().arrow.transform.rotation);

				// ... enable it
				b.SetActive(true);

				// ... increment launch stat
				GetComponent<StatsScript>().SetLaunchValue(
					GetComponent<StatsScript>().GetLaunchValue() + 1);

				// ... ignore the rest of the boats
				break;
			}
		}
	} // end of LaunchBoats()


	//
	internal void RestartLaunchSequence()
	{
		launchNumber = 0;
		restartTimer = defaultTime;
	}
}
