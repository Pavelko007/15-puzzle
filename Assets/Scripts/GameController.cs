using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject TilePrefab;
    public static GameController Instantse;

    void Awake()
    {
        Instantse = this;
    }

    // Use this for initialization
	void Start ()
	{
	    var tileSize = TilePrefab.GetComponent<Tile>().GetSize();

	    Vector3 upperLeftPos = Camera.main.ViewportToWorldPoint(Vector3.up) + (Vector3.down+Vector3.right)*tileSize/2;
	    upperLeftPos.z = 0;
	    gridSize = 4;
	    for (int row = 0; row < gridSize; row++)
	    {
	        for (int col = 0; col < gridSize; col++)
	        {
	            int cellNum = col + row * gridSize + 1;

	            if (cellNum == 16)
	            {
	                cellToTileDict[16] = null;
	            }
	            else
	            {
	                var tile = Instantiate(TilePrefab);
	                tile.transform.position = upperLeftPos 
	                                          + row*Vector3.right * (tileSize) 
	                                          + col*Vector3.down * tileSize;
	                cellToTileDict[cellNum] = tile.GetComponent<Tile>();
	            }
	        }
	    }
	}

    private Dictionary<int, Tile> cellToTileDict = new Dictionary<int, Tile>();
    private int gridSize;

    public void OnTileClicked(Tile tile)
    {
        //find the cell number of clicked tile
        int cellNumber = cellToTileDict.Where(x=>x.Value == tile).Select(x=>x.Key).First();

        //find all adjacent cells
        List<int> adjucentCells = GetAdjucentCells(cellNumber);
        foreach (var cell in adjucentCells)
        {
            Debug.Log(cell);
        }

        //if any adjucent cell is empty then move the tile to this cells
        foreach (var cell in adjucentCells)  {
            if (null == cellToTileDict[cell])
            {
                MoveTileToCell(cell);
            }
        }
    }

    private void MoveTileToCell(int cell)
    {
        throw new System.NotImplementedException();
    }

    private List<int> GetAdjucentCells(int cellNumber)
    {
        throw new System.NotImplementedException();
    }
}
