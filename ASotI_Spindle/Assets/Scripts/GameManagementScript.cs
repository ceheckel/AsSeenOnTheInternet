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
		ChangeCanvasOrientation();

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

	// changes the spacing of the stat tracker and end-game text based on 
	// from which device the game is being run
	internal void ChangeCanvasOrientation()
	{
		GameObject t1 = GameObject.Find("Better Luck");
		GameObject t2 = GameObject.Find("Play Again");
		GameObject t3 = GameObject.Find("Stats");
		if (t1 == null || t2 == null || t3 == null) { return; }

		//Check if we are running either in the Unity editor or in a standalone build.
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
		Vector3 p1 = new Vector3(533.5f, 754.5f, 0);
		Vector3 p2 = new Vector3(533.5f, 154.5f, 0);
		Vector3 p3 = new Vector3(133.5f, 604.5f, 0);
		
		t1.GetComponent<RectTransform>().SetPositionAndRotation(p1, t1.GetComponent<RectTransform>().rotation);
		t2.GetComponent<RectTransform>().SetPositionAndRotation(p2, t2.GetComponent<RectTransform>().rotation);
		t3.GetComponent<RectTransform>().SetPositionAndRotation(p3, t3.GetComponent<RectTransform>().rotation);

		//Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		float sh = Screen.height;
		float sw = Screen.width;
		Vector3 p1 = new Vector3(sw/2, sh-200, 0);
		Vector3 p2 = new Vector3(sw/2, 200, 0);
		Vector3 p3 = new Vector3(100, sh/2, 0);

		t1.GetComponent<RectTransform>().SetPositionAndRotation(p1, t1.GetComponent<RectTransform>().rotation);
		t2.GetComponent<RectTransform>().SetPositionAndRotation(p2, t2.GetComponent<RectTransform>().rotation);
		t3.GetComponent<RectTransform>().SetPositionAndRotation(p3, t3.GetComponent<RectTransform>().rotation);
#endif //End of mobile platform dependendent compilation section started above with #elif
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
