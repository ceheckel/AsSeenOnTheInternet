using System.Collections.Generic;
using UnityEngine;

public class LayTilesScript : MonoBehaviour {

	public GameObject tile; // object to be copied across the grid
	public GameObject arrow; // reference to the launch arrow (for position reset)
	public Sprite[] sprites; // list of sprites that makeup the play field
	public int gridWidth; // number of tiles that make up the x-axis of the grid
	public int gridHeight; // number of tiles that make up the y-axis of the grid
	public float levelDifficulty; // used to adjust number of mid-field obstacles

	private List<GameObject> tiles; // used to track the tiles on the field
	private int[] openingIndex = new int[4]; // used to track the field openings
	// openingIndex holds the coordinates of the upper and lower gap edges
	// [0] -> upper left gap edge
	// [1] -> upper right gap edge
	// [2] -> lower left gap edge
	// [3] -> lower right gap edge

	
	private void Start()
	{
		tiles = new List<GameObject>();

		// randomly choose where the gaps start
		// gaps end shortly after starting
		openingIndex[0] = Random.Range(1, gridWidth - 6);
		openingIndex[1] = openingIndex[0] + 5;
		openingIndex[2] = Random.Range(1, gridWidth - 6);
		openingIndex[3] = openingIndex[2] + 5;

		// for each tile on the grid ...
		for (int x = 0; x < gridWidth; x += 1)
		{
			for (int y = 0; y < gridHeight; y += 1)
			{
				// ...create an obj for said tile
				GameObject obj = (GameObject)Instantiate(tile);

				// generate object information (sprite, name, position, parent, collider)
				obj.GetComponent<SpriteRenderer>().sprite = GenerateSprite(x, y);
				obj.name = "Tile [" + x + "," + y + "]";
				obj.transform.SetPositionAndRotation(new Vector2(x,y), Quaternion.identity);
				obj.transform.parent = tile.transform.parent;
				obj.AddComponent<PolygonCollider2D>();

				// if current tile is an "open water" tile, set it as inactive
				if (obj.GetComponent<SpriteRenderer>().sprite.name.Equals("ow"))
				{
					obj.SetActive(false);
				}
				// otherwise, make it active
				else { obj.SetActive(true); }

				// add new tile to the list of existing tiles
				tiles.Add(obj);
			}
		} // end of "for each tile on grid"

		// move launch arrow to middle of bottom gap
		arrow.transform.Translate(new Vector2(openingIndex[2]+3, 0));
	} // end of Start()
	

	// method that gives each tile on the play field a sprite dependant on position
	// x	- position along the x axis of the grid
	// y	- position along the y axis of the grid
	Sprite GenerateSprite(int x, int y)
	{
		// assume sprite will be "open water" by default
		Sprite s = sprites[4];

		// start by generating the sprites around the edge of the screen
		// sprites on the left side of the screen
		if (x == 0f)
		{
			if (y == 0f) { s = sprites[6]; } // bottom left corner
			else if (y == gridHeight - 1) { s = sprites[0]; } // upper left corner
			else { s = sprites[3]; } // middle pieces on left side
		}
		// sprites on the right side of the screen
		else if (x == gridWidth-1)
		{
			if (y == 0f) { s = sprites[8]; } // lower right corner
			else if (y == gridHeight - 1) { s = sprites[2]; } // upper right corner
			else { s = sprites[5]; } // middle pieces on right side
		}
		// sprites on the bottom of the screen
		else if (y == 0f)
		{
			// bottom left corner is already drawn
			// bottom right corner is already drawn
			if (x == openingIndex[2]) { s = sprites[11]; } // left side of bottom gap
			else if (x == openingIndex[3]) { s = sprites[12]; } // right side of bottom gap
			else if (x > openingIndex[2] && x < openingIndex[3]) { s = sprites[4]; } // gap
			else { s = sprites[7]; } // middle pieces on bottom (not in gap)
		}
		// sprites on the top of the screen
		else if (y == gridHeight-1)
		{
			// upper left corner is already drawn
			// upper right corner is already drawn
			if (x == openingIndex[0]) { s = sprites[9]; } // left side of upper gap
			else if (x == openingIndex[1]) { s = sprites[10]; } // right side of upper gap
			else if (x > openingIndex[0] && x < openingIndex[1]) { s = sprites[4]; } // gap
			else { s = sprites[1]; } // middle pieces on top (not in gap)
		}
		// sprites in the middle of the play field
		else if (Random.Range(0,25) < levelDifficulty)
		{
			// prevent tiles from spawning in front of opening
			if (!(y < 4 && x > openingIndex[2] && x < openingIndex[3]))
			{
				// create random obstacle 
				s = sprites[Random.Range(13, 33)];
			}
		}
        
		return s;
	} // end of GenerateSprite()


	// method called when an object from the in-field needs to change its sprite
	// typically called after collision
	// obj	- tile from the in-field that needs a sprite change
	internal void ChangeSprite(GameObject obj)
	{
		SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

		// for each of the possible in-field sprites ...
		for (int i = 13; i < 33; i += 1)
		{
			// ... find the sprite used by this object ...
			if (sr.sprite.name.Equals(sprites[i].name))
			{
				// stop rendering the sprite while changing
				sr.enabled = false;

				// ... check to see if this object is on the final stage of breaking ...
				if (i % 4 == 0) { sr.sprite = sprites[4]; } // ... if so, make "open water"
				else { sr.sprite = sprites[i + 1]; } // ... if not, break it down by one

				// re-enable sprite rendering before return
				sr.enabled = true;
				break;
			}
		}
	} // end of ChangeSprite()
}