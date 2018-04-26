using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    public GameObject TilePrefab;
    public static GameController Instantse;

    private Dictionary<int, Tile> cellToTileDict = new Dictionary<int, Tile>();
    private int gridSize = 4;

    void Awake()
    {
        Instantse = this;
#if UNITY_EDITOR
#endif
    }


    void Start ()
	{
	    var tileSize = TilePrefab.GetComponent<Tile>().GetSize();

	    Vector3 upperLeftPos = Camera.main.ViewportToWorldPoint(Vector3.up) + (Vector3.down+Vector3.right)*tileSize/2;
	    upperLeftPos.z = 0;
	    
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

    public void OnTileClicked(Tile tile)
    {
        //find the cell number of clicked tile
        int cellNumber = cellToTileDict.Where(x=>x.Value == tile).Select(x=>x.Key).First();

        //find all adjacent cells
        List<int> adjucentCells = GetAdjacentCells(cellNumber);
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

    private List<int> GetAdjacentCells(int cellNumber)
    {
        var result = new List<int>();

        int row = (cellNumber - 1) / gridSize;
        int col = (cellNumber - 1) % gridSize; 

        //upper
        {
            var newRow = row - 1;
            if (newRow >= 0)
            {
                result.Add(GetTileNumber(col, newRow));
            }
        }

        //lower
        {
            var newRow = row + 1;
            if (newRow < gridSize)
            {
                result.Add(GetTileNumber(col, newRow));
            }
        }
        //to the right 
        {
            var newCol = col + 1;
            if (newCol < gridSize)
            {
                result.Add(GetTileNumber(newCol, row));
            }
        }
        //to the left
        {
            var newCol = col - 1;
            if (newCol >= 0)
            {
                result.Add(GetTileNumber(newCol, row));
            }
        }
        return result;
    }

    private int GetTileNumber(int col, int row)
    {
        return col + gridSize*row + 1;
    }
}
