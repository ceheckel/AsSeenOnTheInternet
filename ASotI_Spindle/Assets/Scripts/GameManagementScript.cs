using System.Collections;
using UnityEngine;

public class GameManagementScript : MonoBehaviour {

	// references
	public static GameManagementScript instance; // singleton
	[HideInInspector]
	public bool gameOn; // globally visible boolean to show game status
	
	private int score;
	private float gameTime;
	private GameObject play; // reference to player object
	private GameObject canv; // reference to the game's end screen
	private GameObject plat; // reference to the platforms container

	// used to initialize any variables or game state before the game starts
	// called only once during the lifetime of the script instance
	// called after all objects are initialized and before any Start functions
	private void Awake()
	{
		if (instance == null) { instance = this; }
		else if (instance != this) { Destroy(gameObject); }

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SetGameOn(true);
		SetScore(0);
		SetTime(0);

		play = GameObject.Find("Player");
		if (play == null) { Debug.LogWarning("GameManagementScript: Player not found"); }

		canv = GameObject.Find("End Game Canvas");
		if (canv == null) { Debug.LogWarning("GameManagementScript: End Game Canvas not found"); }
		canv.SetActive(false);

		plat = GameObject.Find("Platforms");
		if (plat == null) { Debug.LogWarning("GameManagementScript: Platform Container not found"); }
	}

	// Listen for Exit Command
	private void Update()
	{
		// Exit Application with "ESC" or "Back" button on mobile
		if (Input.GetKeyDown(KeyCode.Escape)) { ExitGame(); }

		// Restart game
		if ((gameOn == false) && (Input.anyKeyDown == true))
		{
			StartCoroutine("LowerEndGame");
		}

		// Update gameTime variable
		if (gameOn) { gameTime += 1 * Time.deltaTime; }
	}

	// Bring up the credits UI after player dies
	internal void RaiseEndGame()
	{
		// stop all movements
		plat.GetComponent<RotatePlatformScript>().SetMoveEnable(false);
		play.GetComponent<BallMovementScript>().Suspend(true);
		SetGameOn(false);

		// raise UI
		canv.SetActive(true);
	}

	// Hide the credits UI after player continues
	IEnumerator LowerEndGame()
	{
		// reset position
		plat.GetComponent<RaisePlatformScript>().IncRaiseValue();
		yield return new WaitForSeconds(3);
		play.transform.SetPositionAndRotation(new Vector3(0,2,-2.75f), play.transform.rotation);

		// enable all movements
		plat.GetComponent<RotatePlatformScript>().SetMoveEnable(true);
		play.GetComponent<BallMovementScript>().Suspend(false);
		SetGameOn(true);

		// reset score
		SetScore(0);
		SetTime(0);

		// lower UI
		canv.SetActive(false);

		StopCoroutine("LowerEndGame");
	}

	// closes the running application
	internal void ExitGame()
	{
		Application.Quit();
	}

	// setter for gameOn variable
	internal void SetGameOn(bool val) { gameOn = val; }
	// setter for the score variable
	internal void SetScore(int val) { score = val; }
	// getter for the score variable
	internal int GetScore() { return score; }
	// setter for the time variable
	internal void SetTime(int val) { gameTime = val; }
	// getter for the time variable
	internal int GetTime() { return (int)gameTime; }

}
