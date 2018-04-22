using System.Collections;
using System.Collections.Generic;
using FifteenPuzzle;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject TilePrefab;
	
    // Use this for initialization
	void Start ()
	{
	    var tileSize = TilePrefab.GetComponent<Tile>().GetSize();

	    Vector3 upperLeftPos = Camera.main.ViewportToWorldPoint(Vector3.up) + (Vector3.down+Vector3.right)*tileSize/2;
	    upperLeftPos.z = 0;
	    for (int row = 0; row < 4; row++)
	    {
	        for (int col = 0; col < 4; col++)
	        {
	            var tile = Instantiate(TilePrefab);
	            tile.transform.position = upperLeftPos 
                    + row*Vector3.right * (tileSize) 
                    + col*Vector3.down * tileSize;
	        }
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
