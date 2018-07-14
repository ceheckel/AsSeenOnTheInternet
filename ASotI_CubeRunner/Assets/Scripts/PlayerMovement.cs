using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody player_rigidbody;
    public float player_xaccel;
    public float player_yaccel;
    public float player_zaccel;

    // Use this for initialization
    void Start () {
        Debug.Log("Game On!");

        // removes gravity when tied to an object
        //player_rigidbody.useGravity = false;

        // pushes the object when game starts
        // player_rigidbody.AddForce(0, 200, 500); // pushes the player up by +200y and +500z

        //player_xaccel = 0f;
        //player_yaccel = 0f;
        //player_zaccel = 1000f;
	}

    // Update is called once per frame
    private void Update()
    {
        // check if player is pressing movement keys
        // this is a better movement technique than checking for input keys in
        // the FixedUpdate method as this does not cause input lag
        if (Input.GetKey("d"))
        {
            player_xaccel = 100f;
        }
        else if (Input.GetKey("a"))
        {
            player_xaccel = -100f;
        } else { player_xaccel = 0f; }

        //if (Input.GetKey("w"))
        //{
        //    player_zaccel = 100f;
        //}
        //else if (Input.GetKey("s"))
        //{
        //    player_zaccel = -100f;
        //} else { player_zaccel = 0f; }
        
    }
    // FixedUpdate is a preferred update method for Unity
    void FixedUpdate () {
        //player_rigidbody.AddForce(0, 0, player_zaccel*(Time.deltaTime)); 
        // continually pushes player down the z-axis
        // Time.deltaTime is the amount of seconds since the engine drew the last frame
        // this prevents fast computers from moving the player much faster than slower computers

        // update player position if keys are being pressed
        if (player_xaccel != 0)
        {
            player_rigidbody.AddForce(player_xaccel * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (player_yaccel != 0)
        {
            player_rigidbody.AddForce(0, player_yaccel * Time.deltaTime, 0);
        }
        if (player_zaccel != 0)
        {
            player_rigidbody.AddForce(0, 0, player_zaccel * Time.deltaTime);
        }

        if (player_rigidbody.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
