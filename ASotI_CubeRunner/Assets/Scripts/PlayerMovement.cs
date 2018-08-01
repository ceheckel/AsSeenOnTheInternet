using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody player_rigidbody;
    public float player_xaccel;
    public float player_yaccel;
    public float player_zaccel;

	// added for mobile support
	private bool moveLeft;
	private bool moveRight;


    // Use this for initialization
    void Start () {
		moveLeft = false;
		moveRight = false;
	}


    // Update is called once per frame
    private void Update()
	{
		moveLeft = false;
		moveRight = false;

		// if project is being run as desktop or WebGL, use keyboard inputs
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		// check if player is pressing movement keys
		if (Input.GetKey("d")) { moveLeft = true; moveRight = false; }
		else if (Input.GetKey("a")) { moveLeft = false; moveRight = true; }
		else { moveLeft = false; moveRight = false; }

		// if project is being run by a mobile device, defer to touchscreen inputs
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		// check to see if screen was touched in last frame
		if (Input.touchCount > 0)
		{
			// if so, determine which half of the screen was touched
			if (Input.touches[0].position.x > (Screen.width/2)) { moveLeft = true; moveRight = false; }
			else if (Input.touches[0].position.x < (Screen.width/2)) { moveLeft = false; moveRight = true; }
			else { moveLeft = false; moveRight = false; }
		}
#endif

		// apply movement
		if (moveLeft) { player_xaccel = 100f; }
		else if (moveRight) { player_xaccel = -100f; }
		else { player_xaccel = 0f; } // don't move
	}


	// FixedUpdate is a preferred update method for Unity
	void FixedUpdate () {
        // Time.deltaTime is the amount of seconds since the engine drew the last frame
        // this prevents fast computers from moving the player much faster than slower computers

        // update player position if keys are being pressed
        if (player_xaccel != 0)
        {
            player_rigidbody.AddForce(player_xaccel * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (player_yaccel != 0)
        {
            player_rigidbody.AddForce(0, player_yaccel * Time.deltaTime, 0);
        }
        if (player_zaccel != 0)
        {
            player_rigidbody.AddForce(0, 0, player_zaccel * Time.deltaTime);
        }

        if (player_rigidbody.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
