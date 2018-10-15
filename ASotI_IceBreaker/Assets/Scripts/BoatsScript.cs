using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatsScript : MonoBehaviour {

	// references
	public float launchDelay; // amount of time between boat launches
	public float defaultTime; // value that restartTimer is set to after launch stage changes
	public int numIBs;	// number of ice breakers to launch
	public int numFrs;	// number of freighters to launch
	public GameObject frTemp; // cloning template for frieghter
	public GameObject ibTemp; // cloning template for ice breaker
	public Sprite[] frSprites; // set of health equivalent freighter sprites

	private List<GameObject> freighters; // tracking list for freighters
	private List<GameObject> iceBreakers;	// tracking list for icebreakers
	private int launchNumber;	// launch stage value
	private float restartTimer; // amount of delay between launches
	private bool boatsCloned; // tracking value for cloning process
	
	//
	void Start()
	{
		freighters = new List<GameObject>();
		iceBreakers = new List<GameObject>();
	}

	// reset references to objects in hierarchy
	internal void InitLevel()
	{
		// determine if boats need to be cloned or found
		if (freighters != null || iceBreakers != null) { GetAllBoats(); }
		else { boatsCloned = false; }

		RestartFreighterSprite();
	}

	// Update is called once per frame
	void Update()
	{
		// when user gives the word, launching IBs
		if (launchNumber == 1)
		{
			StartCoroutine(LaunchBoatsRepeat(iceBreakers, launchDelay));

			// prevent relaunch of IBs
			launchNumber += 1;
		}
		else if (launchNumber == 2)
		{
			restartTimer -= Time.deltaTime;
			if (restartTimer <= 0) { launchNumber += 1; }
		}
		// when user gives the word, launching FRs
		else if (launchNumber == 4)
		{
			StartCoroutine(LaunchBoatsRepeat(freighters, launchDelay));

			// prevent relaunch of IBs
			launchNumber += 1;
		}
	}

	//
	private void FixedUpdate()
	{
		// check for start signal, launch stage, and active scene
		if ((launchNumber == 0 || launchNumber == 3) && Input.anyKey &&
			SceneManager.GetActiveScene().name.Equals("LevelScene"))
		{
			launchNumber += 1;
			restartTimer = defaultTime;
		}
	}

	// clone the freighters and icebreakers as many times as specified by numFR and numIB
	// set properties relevant to each ship type
	internal void CreateBoats()
	{
		//// prevent multiple boat sets
		//if (boatsCloned) { return; }

		// clone the freighter "numFrs" number of times
		for (int i = 0; i < numFrs; i += 1)
		{
			// clone the freighter
			GameObject obj = Instantiate(frTemp) as GameObject;

			// set properties
			obj.name = "FR [" + i + "]";
			obj.transform.parent = frTemp.transform.parent;
			obj.SetActive(false);

			// add to tracking list
			freighters.Add(obj);
		}

		// clone the ice breakers "numIBs" number of times
		for (int i = 0; i < numIBs; i += 1)
		{
			// clone the IB
			GameObject obj = Instantiate(ibTemp) as GameObject;

			// set properties
			obj.name = "IB [" + i + "]";
			obj.transform.parent = ibTemp.transform.parent;
			obj.SetActive(false);

			// add to tracking list
			iceBreakers.Add(obj);
		}

		//boatsCloned = true;
	} // end of CreateBoats()

	// changes the sprite on the specified freighter
	// used after freighter collides with ice
	internal void ChangeFreighter(GameObject fr)
	{
		fr.GetComponent<Animator>().SetInteger("Health",
			fr.GetComponent<Animator>().GetInteger("Health") - 1);

		if (fr.GetComponent<Animator>().GetInteger("Health") < 0)
		{
			Invoke("Sink", 2f);
		}
	}

	// reset each freighter's sprite to have the "max health" sprite
	internal void RestartFreighterSprite()
	{
		foreach (GameObject fr in freighters)
		{
			//fr.GetComponent<SpriteRenderer>().sprite = frSprites[0];
			fr.GetComponent<Animator>().SetInteger("Health", 4);
		}
	}

	// loads the game over scene
	private void Sink()
	{
		// move to credits
		gameObject.GetComponent<SceneManagerScript>().LoadLevel(3);
	}

	// sets all boats to active in hierarchy
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

	// stops the boats from launching
	// resets launch sequence
	internal void StopLaunch()
	{
		StopAllCoroutines();
		RestartLaunchSequence();
	}

	// begin launching boats
	internal void LaunchBoats(List<GameObject> boats)
	{
		// for each boat in the provided list ...
		foreach (GameObject b in boats)
		{
			// ... find a boat that is disabled and unfinished ...
			if ((!b.activeInHierarchy) /*&&
				(!b.GetComponent<MovementScript>().IsFinished())*/)
			{
				// ... move it to the launch position
				b.transform.SetPositionAndRotation(
					GetComponent<ArrowScript>().arrow.transform.position,
					GetComponent<ArrowScript>().arrow.transform.rotation);

				// ... enable it
				b.SetActive(true);

				// ... increment stats
				GetComponent<StatsScript>().SetLaunchValue(
					GetComponent<StatsScript>().GetLaunchValue() + 1);
				GetComponent<StatsScript>().SetScoreValue(
					GetComponent<StatsScript>().GetScoreValue() + 10);

				// ... ignore the rest of the boats
				break;
			}
		}
	} // end of LaunchBoats()

	// changes the launch stage value to zero and restartTimer to default time
	internal void RestartLaunchSequence()
	{
		launchNumber = 0;
		restartTimer = defaultTime;
	}

	// returns the current stage value of the launch sequence
	internal int GetLaunchSequence()
	{
		return launchNumber;
	}

	// reset the tracking arrays for both ship types
	internal void GetAllBoats()
	{
		Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		iceBreakers.Clear();
		freighters.Clear();

		// for each object in the hierarchy ...
		for (int i = 0; i < array.Length; i += 1)
		{
			// ... if it is an icebreaker, add it to the tracking array
			if (array[i].name.Contains("IB ["))
			{
				iceBreakers.Add((GameObject)array[i]);
			}
			// ... if it is a freighter, add it to the tracking array
			else if (array[i].name.Contains("FR ["))
			{
				freighters.Add((GameObject)array[i]);
			}
		}
	}

	// returns true if any freighters have passed the the upper gap
	internal bool AnyFinished()
	{
		foreach (GameObject fr in freighters)
		{
			if (fr.GetComponent<MovementScript>().IsFinished())
				return true;
		}

		return false;
	}

	// changes the finished status of all freighters in the tracking array to be false
	internal void RestartFinishStatus()
	{
		foreach (GameObject fr in freighters)
		{
			fr.GetComponent<MovementScript>().SetFinished(false);
		}
	}
}
