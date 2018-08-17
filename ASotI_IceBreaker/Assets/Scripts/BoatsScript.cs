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
	private int launchNumber;
	private float restartTimer;
	private bool boatsCloned;


	//
	void Start()
	{
		freighters = new List<GameObject>();
		iceBreakers = new List<GameObject>();
	}


	//
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


	// 
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


	//
	internal void ChangeFreighter(GameObject fr)
	{
		fr.GetComponent<Animator>().SetInteger("Health",
			fr.GetComponent<Animator>().GetInteger("Health") - 1);

		if (fr.GetComponent<Animator>().GetInteger("Health") < 0)
		{
			Invoke("Sink", 2f);
		}
	}


	//
	internal void RestartFreighterSprite()
	{
		foreach (GameObject fr in freighters)
		{
			//fr.GetComponent<SpriteRenderer>().sprite = frSprites[0];
			fr.GetComponent<Animator>().SetInteger("Health", 4);
		}
	}


	//
	private void Sink()
	{
		// move to credits
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
	internal void StopLaunch()
	{
		StopAllCoroutines();
		RestartLaunchSequence();
	}


	//
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


	//
	internal void RestartLaunchSequence()
	{
		launchNumber = 0;
		restartTimer = defaultTime;
	}


	//
	internal int GetLaunchSequence()
	{
		return launchNumber;
	}


	//
	internal void GetAllBoats()
	{
		Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		iceBreakers.Clear();
		freighters.Clear();

		for (int i = 0; i < array.Length; i += 1)
		{
			if (array[i].name.Contains("IB ["))
			{
				iceBreakers.Add((GameObject)array[i]);
			}
			else if (array[i].name.Contains("FR ["))
			{
				freighters.Add((GameObject)array[i]);
			}
		}
	}


	//
	internal bool AnyFinished()
	{
		foreach (GameObject fr in freighters)
		{
			if (fr.GetComponent<MovementScript>().IsFinished())
				return true;
		}

		return false;
	}


	//
	internal void RestartFinishStatus()
	{
		foreach (GameObject fr in freighters)
		{
			fr.GetComponent<MovementScript>().SetFinished(false);
		}
	}
}
