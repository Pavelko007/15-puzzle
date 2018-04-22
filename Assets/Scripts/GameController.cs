using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject TilePrefab;
	
    // Use this for initialization
	void Start ()
	{
	    var upperLeftPos = Vector2.zero;
	    for (int row = 0; row < 4; row++)
	    {
	        for (int col = 0; col < 4; col++)
	        {
	            var tile = Instantiate(TilePrefab);
	            float tileSize = 1;
	            tile.transform.position = upperLeftPos 
                    + row*Vector2.right * (tileSize) 
                    + col*Vector2.down * tileSize;
	        }
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
