using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

	// references
    public int isGameOver = 0; 
    public float timeBetweenScenes; // delay between death and reload
    public GameObject completeLevelUI;

	public void Update()
	{
		// Exit Application with "ESC" or "Back" button on mobile
		if (Input.GetKeyDown(KeyCode.Escape)) { ExitGame(); }
	}

	// Raise end-gmae mechanics on player death
	// function marks game as ended, then reloads the current scene after some time
	public void EndGame()
    {
        // if game is on when function is called, perform end game sequence
        if (isGameOver == 0)
        {
            // Display Game over on UI

            // Reset game
            isGameOver = 1;
            //Restart(); // calls the restart method, but has no delay
            Invoke("Restart", timeBetweenScenes); // calls the Restart method with a delay of "timeBetweenScenes"
        }
    }

	// Loads the currently active scene
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // loads the currently active scene using a reference by name
    }

	// raises the level complete text
	// does not load next level
    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true); // enable end level screen
	}

	// closes the running application
	internal void ExitGame()
	{
		Application.Quit();
	}
}
