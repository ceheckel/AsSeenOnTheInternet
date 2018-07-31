using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManagementScript : MonoBehaviour
{

	public GameObject launchArrow; // used for orientation at launch
	public GameObject ibTemp; // ice breaker template
	public GameObject frTemp; // freighter template
	public int numIB; // number of ice breakers
	public int numFr; // number of freighters
	public float launchDelay; // amount of time between boat launches
	public Sprite[] boatSprites; // set of sprites for the Freighter

	private float inputCooldown; // used to prevent freighter launch with breakers
	private int launchNumber; // used to track which boats to launch and when
	private List<GameObject> breakers; // used to track ice breakers in hierarchy
	private List<GameObject> freighters; // used to track freighters in hierarchy


	// Use this for initialization
	private void Start()
	{
		inputCooldown = 0f;

		// initialize the launch number to 0 (game has not started)
		launchNumber = 0;

		frTemp.GetComponent<SpriteRenderer>().sprite = boatSprites[0];

		CopyBoat(ibTemp, numIB, ref breakers);
		CopyBoat(frTemp, numFr, ref freighters);
	}


	void FixedUpdate()
	{
		// adjust cooldown timer if activated
		if (inputCooldown > 0) { inputCooldown -= Time.deltaTime; }

		// when user gives first input ...
		if (Input.anyKey && (launchNumber == 0) && (inputCooldown <= 0))
		{
			launchNumber = 1;
			inputCooldown = 2.0f;

			// ... launch ice breakers
			StartCoroutine(LaunchBoatsRepeat(breakers, launchDelay));
		}
		// when user gives second input ...
		else if (Input.anyKey && (launchNumber == 1) && (inputCooldown <= 0))
		{
			launchNumber = 2;
			inputCooldown = 2.0f; // not needed, but whatever

			// ... launch freighters
			StartCoroutine(LaunchBoatsRepeat(freighters, launchDelay));
		}
	}


	// wrapper method used instead of InvokeRepeating due to parameter needs
	// This technique is a workaround
	IEnumerator LaunchBoatsRepeat(List<GameObject> boats, float launchDelay)
	{
		while(true)
		{
			LaunchBoats(boats);
			yield return new WaitForSeconds(launchDelay);
		}
	}


	void LaunchBoats(List<GameObject> boats)
	{
		// for each boat in the list of boats ...
		for (int i = 0; i < boats.Count; i += 1)
		{
			// ... if the boat is not active and unfinished ...
			if ((!boats[i].activeInHierarchy) && 
				(!boats[i].GetComponent<BoatMovementScript>().IsFinished()))
			{
				// ... set boat's new position and rotation
				boats[i].transform.SetPositionAndRotation(
				new Vector2(
					launchArrow.transform.position.x,
					launchArrow.transform.position.y),
				launchArrow.transform.rotation);

				// ... set boat as active
				boats[i].SetActive(true);

				// update stat
				gameObject.GetComponent<StatManagementScript>().IncrementCurrentLStat(1);

				// ignore other boats; we set one going already
				break;
			}
		}
	}


	// creates copies of a supplies game object
	// boatTemplate	- GameObject in hierarchy that will be copied
	// numBoats		- final number of boats desired
	// boats		- reference to a list to which we add the new boats
	private void CopyBoat(GameObject boatTemplate, int numBoats, ref List<GameObject> boats)
	{

		// create a list to track the boats
		boats = new List<GameObject>
		{
			// add the template boat to the list
			boatTemplate
		};

		// set template as inactive
		boats[0].SetActive(false);

		// for each of the potential boats (excluding the template boat) ...
		for (int i = 1; i < numBoats; i += 1)
		{
			// ... create a new object for the boat, and set as inactive
			GameObject newBoat = (GameObject)Instantiate(boatTemplate);
			newBoat.SetActive(false);

			// ... set defualt position for boats
			newBoat.transform.SetPositionAndRotation(
				new Vector3(0, 0, 0), Quaternion.identity);

			// ... give boat name and place in hierarchy based on template
			if (boatTemplate.name.Equals("Boat[0]"))
			{
				newBoat.name = "Boat[" + i + "]";
				newBoat.transform.parent = boatTemplate.transform.parent;
			}
			else if (boatTemplate.name.Equals("Freighter[0]"))
			{
				newBoat.name = "Freighter[" + i + "]";
				newBoat.transform.parent = boatTemplate.transform.parent;
			}
			else
			{
				Debug.LogError("Name of boatTemplate not recognized\n\tBoatManagementScript.Start()");
			}

			// ... add boat to list
			boats.Add(newBoat);
		}
	} // end of CopyBoat()


	internal void ChangeFreighterSprite(GameObject curr)
	{
		Sprite s = curr.GetComponent<SpriteRenderer>().sprite;
		Debug.Log("curr Sprite: " + s.name);

		// find the curr sprit ein the list of freighter sprites
		for (int i = 0; i < boatSprites.Length-1; i += 1)
		{
			// if found, the freighter has some health remaining
			if (s.name.Equals(boatSprites[i].name))
			{
				// get new sprite
				Debug.Log("Sprite Found decrementing to: " + boatSprites[i + 1].name);
				s = boatSprites[i + 1];

				// prevent excessive updates
				break;
			}
		}

		// if sprite was not updated, freighter has no health remaining.  End game.
		if (s == curr.GetComponent<SpriteRenderer>().sprite)
		{
			Debug.Log("Freighter sank");

			gameObject.GetComponent<StatManagementScript>().IncrementCurrentScore(-1000);
			gameObject.GetComponent<StatManagementScript>().EndGame();
		}
		// reflect new sprite changes
		curr.GetComponent<SpriteRenderer>().sprite = s;
	} // end of ChangeSprite()
}
