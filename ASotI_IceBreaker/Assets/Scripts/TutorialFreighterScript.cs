using UnityEngine;

public class TutorialFreighterScript : MonoBehaviour {

	public Sprite[] sprites;


	// Use this for initialization
	void Start () {
		InvokeRepeating("ChangeSprite", 2f, 2f);
	}

	private void ChangeSprite()
	{
		Sprite s = gameObject.GetComponent<SpriteRenderer>().sprite;

		for (int i = 0; i < sprites.Length; i += 1)
		{
			if (s == sprites[i])
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(i + 1) % sprites.Length];
			}
		}
	}
}
