using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
	
	public void StartGame() { SceneManager.LoadScene("LevelScene"); } // loads the main level scene
	public void Tutorial() { SceneManager.LoadScene("TutorialScene"); } // loads the tutorial scene
	public void Leaderboard() { SceneManager.LoadScene("LeaderboardScene"); } // loads the leaderboard scene
	public void OtherGames() { Application.OpenURL("https://github.com/ceheckel/AsSeenOnTheInternet"); }

	public void RestartGame() { SceneManager.LoadScene("MainMenuScene"); }
	public void ExitGame() { Application.Quit(); }
}
