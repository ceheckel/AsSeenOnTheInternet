using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	// references
    public Transform player; // player position
    public Text scoreText; // text object for score
	
	// Update is called once per frame
	void Update () {
        // assign player position as UI text with a minimum of four digit
        scoreText.text = player.position.z.ToString("0000");
	}
}
