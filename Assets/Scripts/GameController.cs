using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject TilePrefab;
    public static GameController Instantse;

    private Dictionary<int, Tile> cellToTileDict = new Dictionary<int, Tile>();
    private int gridSize = 4;
    private float cellSize;
    private Vector3 upperLeftPos;

    void Awake()
    {
        Instantse = this;
#if UNITY_EDITOR
#endif
    }

    void Start ()
	{
	    cellSize = TilePrefab.GetComponent<Tile>().GetSize();

	    upperLeftPos = Camera.main.ViewportToWorldPoint(Vector3.up) + (Vector3.down+Vector3.right)*cellSize/2;
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
	                tile.transform.position = GetTilePosition(col, row);
	                cellToTileDict[cellNum] = tile.GetComponent<Tile>();
	            }
	        }
	    }
	}

    private Vector3 GetTilePosition(int col, int row)
    {
        return upperLeftPos
               + col * Vector3.right * cellSize
               + row * Vector3.down * cellSize;
    }

    public void OnTileClicked(Tile tile)
    {
        //find the cell number of clicked tile
        int tileCellNum = cellToTileDict.Where(x=>x.Value == tile).Select(x=>x.Key).First();

        //find all adjacent cells
        List<int> adjucentCells = GetAdjacentCells(tileCellNum );
     

        //if any adjucent cell is empty then move the tile to this cells
        foreach (var cell in adjucentCells)  {
            if (null == cellToTileDict[cell])
            {
                MoveTileToCell(cell, tileCellNum );
            }
        }
    }

    private void MoveTileToCell(int emptyCellNum, int tileCellNum)
    {
        Tile tileToMove = cellToTileDict[tileCellNum];

        //update references
        cellToTileDict[emptyCellNum] = cellToTileDict[tileCellNum];
        cellToTileDict[tileCellNum] = null;

        //change tiles postion
        tileToMove.transform.position = GetTilePosition(GetCol(emptyCellNum), GetRow(emptyCellNum));
    }

    private List<int> GetAdjacentCells(int cellNumber)
    {
        var result = new List<int>();

        int row = GetRow(cellNumber);
        int col = GetCol(cellNumber); 

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

    private int GetCol(int cellNumber)
    {
        return (cellNumber - 1) % gridSize;
    }

    private int GetRow(int cellNumber)
    {
        return (cellNumber - 1) / gridSize;
    }

    private int GetTileNumber(int col, int row)
    {
        return col + gridSize*row + 1;
    }
}
