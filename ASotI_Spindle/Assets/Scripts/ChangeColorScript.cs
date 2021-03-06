﻿using UnityEngine;

public class ChangeColorScript : MonoBehaviour
{
	public bool startColorRed = true;
	public Renderer rend;

	private readonly int stageSize = 50;
	private readonly Color[] colors = new Color[7];
	private int currentColor = 0;

	private GameObject gm; // reference to the game manager

	void Start()
	{
		gm = GameObject.Find("GameController");
		if (gm == null) { Debug.LogWarning("ChangeColorScript: GameController not found"); }

		rend = GetComponent<Renderer>();

		if (startColorRed == true)
		{
			colors[0] = Color.red;
			colors[1] = new Color(1, 0.65f, 0, 1);      // orange rgba(255,165,0,1 )
			colors[2] = Color.yellow;
			colors[3] = Color.green;
			colors[4] = Color.blue;
			colors[5] = new Color(0.29f, 0, 0.51f, 1);	// indigo rgba(75,0,130,1)
			colors[6] = new Color(0.47f, 0.36f, 0.31f, 1);	// violet rgba(120,92,80,1)
		}
		else
		{
			colors[6] = Color.red;
			colors[5] = new Color(1, 0.65f, 0, 1);      // orange
			colors[4] = Color.yellow;
			colors[3] = Color.green;
			colors[2] = Color.blue;
			colors[1] = new Color(0.29f, 0, 130, 1);  // indigo
			colors[0] = new Color(0.50f, 0, 0.50f, 1);  // violet
		}
	}

	void Update()
	{
		UpdateStage();

		// change the render color of the object based on stage value and score
		rend.material.color = Color.Lerp
			(
				colors[currentColor],
				colors[NextColorIndex()],
				(gm.GetComponent<GameManagementScript>().GetScore() % (float)stageSize) / stageSize
			);
	}

	// round robin incrementer for currentColor variable
	// rolls over at 7
	private int NextColorIndex() { return (currentColor + 1) % 7; }

	// change the currentColor variable based on the current score
	private void UpdateStage()
	{
		// current color index equals the number of stageSizes in the current score
		// when the current score reaches '80' the current color should reset to '0'
		currentColor = (gm.GetComponent<GameManagementScript>().GetScore() / stageSize) % 7;
	}
}
