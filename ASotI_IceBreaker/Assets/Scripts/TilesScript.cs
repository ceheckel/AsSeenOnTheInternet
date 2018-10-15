using UnityEngine;
using System.Collections.Generic;

public class TilesScript : MonoBehaviour {

	// references
	public int gridHeight; // number of rows on the field
	public int gridWidth; // number of columns on the field
	public Sprite[] sprites; // set of in-field obstacle sprites
	public float levelDifficulty; // used when generating obstacles

	private List<GameObject> tiles; // list of in-field tiles
	private GameObject tile; // tile object to be cloned
	private bool tilesCloned;
	private readonly int[] openingIndex = new int[4]; // used to track the field openings
											 // openingIndex holds the coordinates of the upper and lower gap edges
											 // [0] -> upper left gap edge
											 // [1] -> upper right gap edge
											 // [2] -> lower left gap edge
											 // [3] -> lower right gap edge

	//
	void Start()
	{
		tiles = new List<GameObject>();
	}

	// create references to objects needed to create levels
	internal void InitLevel()
	{
		// find tile template
		tile = GameObject.FindGameObjectWithTag("Obstacle");
		if (tile == null) { Debug.LogWarning("No tile object found"); }

		// determine if their are tiles in the hierarchy
		if (tiles != null) { GetAllTiles(); }

		// reset gap openings
		openingIndex[0] = 0; openingIndex[1] = 0;
		openingIndex[2] = 0; openingIndex[3] = 0;

		// prevent tile cloning process
		tilesCloned = false;
	}

	// clone tile template to fill the game grid
	internal void CreateTiles()
	{
		// prevent multiple tile sets
		if (tilesCloned) { return; }
		
		// for each position on the grid ...
		for (int y = 0; y < gridHeight; y += 1)
		{
			for (int x = 0; x < gridWidth; x += 1)
			{
				// ... clone the tile
				GameObject obj = Instantiate(tile) as GameObject;

				// ... set additional properties
				obj.name = "Tile[" + x + "," + y + "]";
				obj.transform.SetPositionAndRotation(
					new Vector2(x, y), Quaternion.identity);
				obj.transform.parent = tile.transform.parent;

				// ... add to tracking list
				tiles.Add(obj);
			}
		}

		// mark cloning process as completed
		tilesCloned = true;
	} // end of CreateTiles()

	// method that gives each tile on the play field a sprite dependant on position
	// x	- position along the x axis of the grid
	// y	- position along the y axis of the grid
	internal Sprite GenerateSprite(int x, int y)
	{
		// assume sprite will be "open water" by default
		Sprite s = sprites[20];

		// start by generating the sprites around the edge of the screen
		// sprites on the left side of the screen
		if (x == 0f)
		{
			if (y == 0f) { s = sprites[22]; } // bottom left corner
			else if (y == gridHeight - 1) { s = sprites[31]; } // upper left corner
			else { s = sprites[24]; } // middle pieces on left side
		}
		// sprites on the right side of the screen
		else if (x == gridWidth - 1)
		{
			if (y == 0f) { s = sprites[23]; } // lower right corner
			else if (y == gridHeight - 1) { s = sprites[32]; } // upper right corner
			else { s = sprites[29]; } // middle pieces on right side
		}
		// sprites on the bottom of the screen
		else if (y == 0f)
		{
			// bottom left corner is already drawn
			// bottom right corner is already drawn
			if (x == openingIndex[2]) { s = sprites[25]; } // left side of bottom gap
			else if (x == openingIndex[3]) { s = sprites[26]; } // right side of bottom gap
			else if (x > openingIndex[2] && x < openingIndex[3]) { s = sprites[20]; } // gap
			else { s = sprites[21]; } // middle pieces on bottom (not in gap)
		}
		// sprites on the top of the screen
		else if (y == gridHeight - 1)
		{
			// upper left corner is already drawn
			// upper right corner is already drawn
			if (x == openingIndex[0]) { s = sprites[27]; } // left side of upper gap
			else if (x == openingIndex[1]) { s = sprites[28]; } // right side of upper gap
			else if (x > openingIndex[0] && x < openingIndex[1]) { s = sprites[20]; } // gap
			else { s = sprites[30]; } // middle pieces on top (not in gap)
		}
		// sprites in the middle of the play field
		else if (Random.Range(0, 25) < levelDifficulty)
		{
			// prevent tiles from spawning in front of opening
			if (!(y < 4 && x > openingIndex[2] && x < openingIndex[3]))
			{
				// create random obstacle 
				s = sprites[Random.Range(0, 20)];
			}
		}

		return s;
	} // end of GenerateSprite()

	// change the sprite of all the non-border tiles on the grid including the openings
	internal void ChangeTiles()
	{
		// safety checks
		if (tiles == null) { Debug.LogWarning("ChangeTiles(): tiles null"); return; }
		if (tiles.Count == 0) { Debug.LogWarning("ChangeTiles(): tiles empty"); return; }

		// get new opening points
		NewOpening();
		
		// foreach tile on the grid ...
		foreach (GameObject t in tiles)
		{
			// ... perform safety check on tile
			if (t == null) { Debug.LogWarning("ChangeTiles(): tile is null"); return; }

			// ... obtain a new sprite from the list of possible sprites
			t.GetComponent<SpriteRenderer>().sprite = GenerateSprite(
				(int)t.transform.position.x,
				(int)t.transform.position.y);

			// ... change polygon collider to resemble new sprite
			// this is not a great way of updating the polygon,
			// but this only occurs on level changes
			if (t.GetComponent<PolygonCollider2D>() != null)
			{
				Destroy(t.GetComponent<PolygonCollider2D>());
			}
			t.AddComponent<PolygonCollider2D>();

			// ... disable "open water" tiles (prevents boat collision on 'nothing')
			if (t.GetComponent<SpriteRenderer>().sprite.name.Equals("ow"))
			{
				t.SetActive(false);
			}
			// ... enable non-openwater tiles (prevent boats from lcipping through obstacles)
			else
			{
				t.SetActive(true);
			}
		}
	} // end of ChangeTiles()

	// deactivate all tiles from the grid
	internal void ClearField()
	{
		// safety checks
		if (tiles == null) { Debug.LogWarning("ClearField(): tiles null");	return; }
		if (tiles.Count == 0) { Debug.LogWarning("ClearField(): tiles empty");	return; }
		
		// for each tile in the grid ...
		foreach (GameObject t in tiles)
		{
			// ... if it is active in the hierarchy, disable it
			if (t.activeInHierarchy) { t.SetActive(false); }
		}
	} // end of ClearField

	// deactivate all non-border tiles from the grid
	internal void OpenField()
	{
		// safety check
		if (tiles == null) { return; }

		// remove boarder openings
		for (int i = 0; i < 4; i += 1) { openingIndex[i] = -1; }

		// fill boarder with solid wall
		foreach (GameObject t in tiles)
		{
			if (t.transform.position.x == 0 ||
				t.transform.position.x == gridWidth-1 ||
				t.transform.position.y == 0 ||
				t.transform.position.y == gridHeight-1)
			{
				GenerateSprite((int)t.transform.position.x, (int)t.transform.position.y);
			}
			else
			{
				// disable tile if not on boarder
				t.SetActive(false);
			}
		}
	} // end of OpenField()

	// generate a new position for the upper and lower border openings
	private void NewOpening()
	{
		openingIndex[0] = Random.Range(1, gridWidth - 7);
		openingIndex[1] = openingIndex[0] + 6;
		openingIndex[2] = Random.Range(1, gridWidth - 7);
		openingIndex[3] = openingIndex[2] + 6;
	}

	// returns the grid value of the center of the lower gap
	// used to place launch arrow
	internal int GetLowerGapCenter() { return openingIndex[2] + 3; }

	// changes the current obstacle sprite with one of a lower durability
	// if the current sprite is at it's lowest value, it is replaces with the open water sprite
	internal void BreakSprite(GameObject tile)
	{
		// for each of the possible in-field sprites ...
		for (int i = 0; i < 20; i += 1)
		{
			// ... if the provided object has a matching sprite ...
			if (tile.GetComponent<SpriteRenderer>().sprite == sprites[i])
			{
				// ... check it's value 
				//		(multiple of four implies object is about to be destroyed)
				if (i % 4 == 0)
				{
					// ... obstacle has no health remaining, change to "open water"
					tile.GetComponent<SpriteRenderer>().sprite = sprites[20];
					tile.SetActive(false);
				}
				else
				{
					// ... obstacle has some health remaining, change to lesser sprite
					tile.GetComponent<SpriteRenderer>().sprite = sprites[i - 1];
				}

				// ... increment score
				// (I don't know why I have to be this explicit, but using just 
				//  "gameObject.GetComponent..." was not returning the correct value)
				GameObject.Find("SceneManager").GetComponent<StatsScript>().SetScoreValue(
					GameObject.Find("SceneManager").GetComponent<StatsScript>().GetScoreValue() + 10);
				
				break;
			}
		}
	} // end of BreakSprite()


	// replaces all objects in the tile tracking array with the tile objects currently in hierarchy
	private void GetAllTiles()
	{
		// find all gameObjects in hierarchy
		Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		// empty tile tracking array
		tiles.Clear();

		// for each gameObject found in Hierarchy ...
		for (int i = 0; i < array.Length; i += 1)
		{ 
			// ... if it is a tile object, 
			if (array[i].name.Contains("Tile["))
			{
				// ... add it to the tracking array
				tiles.Add((GameObject)array[i]);
			}
		}
	} // end of GetAllTiles()
}
