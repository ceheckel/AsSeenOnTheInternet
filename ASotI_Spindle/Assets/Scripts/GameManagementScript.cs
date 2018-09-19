using System.Collections;
using UnityEngine;

public class GameManagementScript : MonoBehaviour {

	// references
	public static GameManagementScript instance; // singleton
	[HideInInspector]
	public bool gameOn; // globally visible boolean to show game status

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

		play = GameObject.Find("Player");
		if (play == null) { Debug.LogWarning("GameManagementScript: Player not found"); }

		canv = GameObject.Find("Canvas");
		if (canv == null) { Debug.LogWarning("GameManagementScript: Canvas not found"); }
		canv.SetActive(false);

		plat = GameObject.Find("Platforms");
		if (plat == null) { Debug.LogWarning("GameManagementScript: Platform Container not found"); }
	}

	// Listen for Exit Command
	private void Update()
	{
		// Exit Application with "ESC" or "Back" button on mobile
		if (Input.GetKeyDown(KeyCode.Escape)) { ExitGame(); }

		if ((gameOn == false) && (Input.anyKeyDown == true))
		{
			StartCoroutine("LowerEndGame");
		}
	}

	// Bring up the credits UI after player dies
	internal void RaiseEndGame()
	{
		// stop all movements
		plat.GetComponent<RotatePlatformScript>().SetMoveEnable(false);
		play.GetComponent<ResetMomentumScript>().Suspend(true);
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
		play.GetComponent<ResetMomentumScript>().Suspend(false);
		SetGameOn(true);

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
}
