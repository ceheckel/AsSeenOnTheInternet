using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreScript : MonoBehaviour {

	// references
	public Text statText;
	public GameObject gc;
	
	// Update is called once per frame
	void Update () {
		if (gc.GetComponent<GameManagementScript>().gameOn)
		{
			statText.text = "Score: " + gc.GetComponent<GameManagementScript>().GetScore() + "\n"
				+ "Time: " + (int)Time.time;
		}
	}
}
