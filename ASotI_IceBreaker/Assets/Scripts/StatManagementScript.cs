using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatManagementScript : MonoBehaviour {
	
	public Text levelstat;
	public Text scorestat;
	public Text launchstat;

	private int levelNumber;
	private float score;
	private int LStat;


	void Start()
	{
		levelNumber = 1;
		score = 0f;
		LStat = 0;
	}


	// Update is called once per frame
	void Update ()
	{
		// display current in-game stats every frame
		levelstat.text = GetCurrentLevel().ToString();
		scorestat.text = GetCurrentScore().ToString();
		launchstat.text = GetCurrentLStat().ToString();

		score -= 1 * Time.deltaTime;
	}


	// loads the main level after resetting the stats
	public void Restart()
	{
		levelNumber = 1;
		score = 0;
		LStat = 0;

		SceneManager.LoadScene("LevelScene");
	}

	// loads the main level without resetting the stats
	// typically used after level completion
	void NewLevel() { SceneManager.LoadScene("LevelScene"); }
	// loads the credits screen
	internal void EndGame() { SceneManager.LoadScene("CreditsScene"); }


	// getter and setter for current in-game level stat
	// setter adds value of param to current local value
	// setter adds value (x localMultiplier) to score stat
	public void IncrementCurrentLevel(int val) { levelNumber += val; IncrementCurrentScore(val*100); }
	public int GetCurrentLevel() { return levelNumber; }

	// getter and setter for current in-game score stat
	// setter adds value of param to current local value
	public void IncrementCurrentScore(int val) { score += val; }
	public int GetCurrentScore() { return (int)score; }

	// getter and setter for current in-game launch stat
	// setter adds value of param to current local value
	// setter adds value (x localMultiplier) to score stat
	public void IncrementCurrentLStat(int val) { LStat += val; IncrementCurrentScore(val*10); }
	public int GetCurrentLStat() { return LStat; }

}
