using UnityEngine;

public class ArrowMovementScript : MonoBehaviour {

	public float rotationSpeed; // used in the arrow oscillation formula
	public float restartTimer; // how long the arrow is static after launch

	private Vector3 maxAngle; // how far to the left the arrow oscillates
	private Vector3 minAngle; // how far to the right the arrow oscillates
	private float calcRotSpeed; // result of the oscillation formula
	private int launchNumber; // stage counter for the boat launches and arrow

	// Use this for initialization
	void Start ()
	{
		// determing minimum and maximum angles for the launch arrow
		maxAngle = new Vector3(0f, 0f, 60f);
		minAngle = new Vector3(0f, 0f, -60f);

		// start at launch zero
		launchNumber = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		// if icebreakers are ready for launch or freighter is ready ...
		if (launchNumber == 0 || launchNumber == 2)
		{
			// ... ocsillate the launch arrow
			calcRotSpeed = (Mathf.Sin(Time.time * rotationSpeed * Mathf.PI * 2.0f) + 1.0f) / 2.0f;
			transform.eulerAngles = Vector3.Lerp(maxAngle, minAngle, calcRotSpeed);
		}
		// if one launch has occurred (ice breakers are launched) ...
		else if (launchNumber == 1)
		{
			// ... start countdown timer for freighter launch
			restartTimer -= Time.deltaTime;

			// ... when the timer reaches zero, start moving the arrow again
			if (restartTimer <= 0) { launchNumber = 2; }
		}
	}

	private void FixedUpdate()
	{
		// check for start signal
		if (Input.anyKey && launchNumber == 0) { launchNumber = 1; }
		if (Input.anyKey && launchNumber == 2) { launchNumber = 3; }
	}
}
