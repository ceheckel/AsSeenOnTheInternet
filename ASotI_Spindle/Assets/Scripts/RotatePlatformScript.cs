using UnityEngine;

public class RotatePlatformScript : MonoBehaviour {

	// references
	public bool moveEnabled;

	private int horizontal;

	// Use this for initialization
	void Start () {
		moveEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		//Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int)(Input.GetAxisRaw("Horizontal"));

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		// check to see if screen was touched in last frame
		if (Input.touchCount > 0)
		{
			// if so, determine which half of the screen was touched
			if (Input.touches[0].position.x < (Screen.width/2)) { horizontal = -1; } // clockwise
			if (Input.touches[0].position.x > (Screen.width/2)) { horizontal = 1; } // countercw
		}
		else { horizontal = 0; }
            
#endif //End of mobile platform dependendent compilation section started above with #elif

		//Check if we have a non-zero value
		if (horizontal != 0) { Rotate(horizontal); }
	} // end of Update

	// rotate the flywheels based on the previously calculated direction
	// only works if moveEnabled is true
	private void Rotate(int direction)
	{
		if (moveEnabled && (direction < 0))
			gameObject.transform.Rotate(Vector3.down);
		if (moveEnabled && (direction > 0))
			gameObject.transform.Rotate(Vector3.up);
	} // end of Rotate

	// setter for the moveEnabled boolean
	// typically called when game is ending or starting
	internal void SetMoveEnable(bool val)
	{
		moveEnabled = val;
	}
}
