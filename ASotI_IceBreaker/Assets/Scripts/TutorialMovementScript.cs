using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMovementScript : MonoBehaviour {

	public void MoveCameraUp()
	{
		gameObject.transform.Translate(new Vector2(0, 20), Space.Self);
	}


	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}
}
