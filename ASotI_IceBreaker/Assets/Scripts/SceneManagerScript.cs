using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public static SceneManagerScript instance;


	// used to initialize any variables or game state before the game starts
	// called only once during the lifetime of the script instance
	// called after all objects are initialized and before any Start functions
	private void Awake()
	{
		if (instance == null) { instance = this; }
		else if (instance != this) { Destroy(gameObject); }

		DontDestroyOnLoad(gameObject);
	}


	// handles the level transitions, typically to and from the main menu
	public void LoadLevel(int level)
	{
		// if param is not a valid level, send to "other games" page
		if (level < 0 || level >= SceneManager.sceneCountInBuildSettings)
		{
			Application.OpenURL("https://www.github.com/ceheckel/AsSeenOnTheInternet/");
		}
		else
		{
			SceneManager.LoadScene(level);
		}
	}


	//
	private void OnLevelWasLoaded(int level)
	{
		switch (level)
		{
			case 0: /* Logic for return to main menu */
				gameObject.GetComponent<StatsScript>().ClearStats(); // reset stats
				gameObject.GetComponent<TilesScript>().ClearField(); // empty in-field
				gameObject.GetComponent<BoatsScript>().SetBoatsActive(false);
				break;

			case 1: /* Logic for transition to tutorial */
				gameObject.GetComponent<BoatsScript>().SetBoatsActive(true);
				gameObject.GetComponent<ArrowScript>().FindArrow();
				gameObject.GetComponent<ArrowScript>().RestartLaunchSequence();
				break;

			case 2: /* Logic for loading of main level */
				gameObject.GetComponent<StatsScript>().FindStats(); // make reference connections

				gameObject.GetComponent<TilesScript>().InitLevel(); // make reference connections
				gameObject.GetComponent<TilesScript>().CreateTiles(); // create tile objects
				gameObject.GetComponent<TilesScript>().ChangeTiles(); // give objects new sprites
				
				gameObject.GetComponent<BoatsScript>().CreateBoats(); // create boat obects
				gameObject.GetComponent<BoatsScript>().SetBoatsActive(false);
				gameObject.GetComponent<BoatsScript>().RestartLaunchSequence();

				gameObject.GetComponent<ArrowScript>().FindArrow(); // make reference connections
				gameObject.GetComponent<ArrowScript>().RelocateArrow(); // move the launch arrow
				gameObject.GetComponent<ArrowScript>().RestartLaunchSequence();
				break;

			case 3: /* Logic for credits screen */
				gameObject.GetComponent<TilesScript>().ClearField(); // empty in-field
				gameObject.GetComponent<TilesScript>().OpenField(); // set border only
				gameObject.GetComponent<BoatsScript>().SetBoatsActive(true);

				//gameObject.GetComponent<BoatsScript>().CreditsBuild(); // create boats for credits

				//gameObject.GetComponent<GUIScript>().CreditsBuild(); // create buttons
				break;
		} // end of switch-case
	} // end of OnLevelWasLoaded()
}
