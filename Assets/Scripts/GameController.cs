using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject TilePrefab;
    public static GameController Instantse;
    public GameObject GameOverPanel;

    private Dictionary<int, Tile> cellToTileDict = new Dictionary<int, Tile>();
    private int gridSize = 4;
    private float cellSize;
    private Vector3 upperLeftPos;
    private int emptyCellNum;
    private bool isPlaying = true;

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
	    
	    CreateTiles();
	    StartNewGame();
	}

    public void StartNewGame()
    {
        isPlaying = true;
        GameOverPanel.SetActive(false);
        ShuffleTiles();
    }

    private void ShuffleTiles()
    {
        int numIterations = 1;
        while (numIterations-- > 0)
        {
            var adjacentCells = GetAdjacentCells(emptyCellNum);
            var cellToMove = adjacentCells[Random.Range(0, adjacentCells.Count)];
            MoveTileToEmptyCell(cellToMove, false);
        }

        foreach (var key in cellToTileDict.Keys)
        {
            var tile = cellToTileDict[key];
            if (tile != null) UpdateTilePosition(tile, key);
        }
    }

    private void CreateTiles()
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                int cellNum = col + row * gridSize + 1;

                if (cellNum == 16)
                {
                    emptyCellNum = 16;
                    cellToTileDict[16] = null;
                }
                else
                {
                    var tileGo = Instantiate(TilePrefab);
                    tileGo.transform.position = GetTilePosition(col, row);
                    var tile = tileGo.GetComponent<Tile>();
                    tile.Number = cellNum;
                    cellToTileDict[cellNum] = tile;
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
        if (!isPlaying) return;

        //find the cell number of clicked tile
        int tileCellNum = cellToTileDict.Where(x=>x.Value == tile).Select(x=>x.Key).First();

        //find all adjacent cells
        List<int> adjucentCells = GetAdjacentCells(tileCellNum );
     
        //if any adjucent cell is empty then move the tile to this cells
        if (adjucentCells.Contains(emptyCellNum))
        {
            MoveTileToEmptyCell(tileCellNum);
            if (IsBoardValid())
            {
                isPlaying = false;
                GameOverPanel.SetActive(true);
            }
        }
    }

    private bool IsBoardValid()
    {
        foreach (var key in cellToTileDict.Keys)
        {
            var tile = cellToTileDict[key];
            if (tile != null)
            {
                if (tile.Number != key) return false;
            }
        }
        return true;
    }

    private void MoveTileToEmptyCell(int tileCellNum, bool updatePosition = true)
    {
        if (updatePosition)
        {
            var tile = cellToTileDict[tileCellNum];
            UpdateTilePosition(tile, emptyCellNum);
        }

        //update references
        cellToTileDict[emptyCellNum] = cellToTileDict[tileCellNum];
        cellToTileDict[tileCellNum] = null;


        emptyCellNum = tileCellNum;
    }

    private void UpdateTilePosition(Tile tile, int cellNumber)
    {
        tile.transform.position = GetTilePosition(GetCol(cellNumber), GetRow(cellNumber));
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
