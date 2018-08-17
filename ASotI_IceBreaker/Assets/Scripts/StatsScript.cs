using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsScript : MonoBehaviour {

	// references
	private GameObject levelValue;
	private GameObject launchValue;
	private GameObject scoreValue;
	private int levelNum;
	private int launchNum;
	private float score;
	

	//
	internal void FindStats()
	{
		GameObject[] stat = GameObject.FindGameObjectsWithTag("Stat");
		foreach (GameObject s in stat)
		{
			if (s.name.Contains("Level - Value")) { levelValue = s; }
			if (s.name.Contains("Launches - Value")) { launchValue = s; }
			if (s.name.Contains("Score - Value")) { scoreValue = s; }
		}
	}


	//
	private void Update()
	{
		// during the game ...
		if (SceneManager.GetActiveScene().name.Equals("LevelScene") ||
			SceneManager.GetActiveScene().name.Equals("CreditsScene"))
		{
			// ... continually update text fields
			levelValue.GetComponent<Text>().text = GetLevelValue().ToString();
			launchValue.GetComponent<Text>().text = GetLaunchValue().ToString();
			scoreValue.GetComponent<Text>().text = ((int)GetScoreValue()).ToString();

			if (SceneManager.GetActiveScene().name.Equals("LevelScene"))
			{
				// ... decrement score with time
				SetScoreValue(GetScoreValue() - Time.deltaTime);
			}
		}
	}


	// functions
	internal void ClearStats()
	{
		SetLevelValue(1);
		SetLaunchValue(0);
		SetScoreValue(0.0f);
	}


	// getter and setter for level number
	internal void SetLevelValue(int val) { levelNum = val; }
	public int GetLevelValue() { return levelNum; }
	// getter and setter for launch number
	internal void SetLaunchValue(int val) { launchNum = val; }
	public int GetLaunchValue() { return launchNum; }
	// getter and setter for score
	internal void SetScoreValue(float val) { score = val; }
	public float GetScoreValue() { return score; }
}
