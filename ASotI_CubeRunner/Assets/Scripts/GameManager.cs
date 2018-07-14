using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public int isGameOver = 0;
    public float timeBetweenScenes;
    public GameObject completeLevelUI;
    
    // custom function
    public void EndGame()
    {
        // if game is on when function is called, perform end game sequence
        if (isGameOver == 0)
        {
            Debug.Log("Game Over");

            // Display Game over on UI

            // Reset game
            isGameOver = 1;
            //Restart(); // calls the restart method, but has no delay
            Invoke("Restart", timeBetweenScenes); // calls the Restart method with a delay of "timeBetweenScenes"
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // loads the currently active scene using a reference by name
    }

    public void CompleteLevel()
    {
        Debug.Log("Level Won");
        completeLevelUI.SetActive(true); // enable end level screen
    }
}
