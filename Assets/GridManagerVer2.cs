using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerVer2 : MonoBehaviour {

    public static GridManagerVer2 instance;

    public GameObject tilePrefab;
    public TileBehaviorVer2[,] tile = new TileBehaviorVer2[5, 5]; // the whole grid(5*5) tiles as an array

    public Sprite[] tileSprite;

    public bool isShifting;
    public bool boardResolved;

    // Use this for initialization
    void Awake()
    {
        instance = GetComponent<GridManagerVer2>();
        FillGrid();
        boardResolved = true;
    }

	void Update()
	{
        if(!isShifting)
        {
            isShifting = true;
            ClearMatch();
            ShiftGrid();
            FillGrid();
            isShifting = false;
        }
	}

    void FillGrid()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!tile[x, y])
                {
                    tile[x, y] = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<TileBehaviorVer2>();
                    tile[x, y].transform.parent = gameObject.transform;
                    List<Sprite> avaliableSprites = new List<Sprite>();
                    avaliableSprites.AddRange(tileSprite);
                    if(x > 0 && y > 0)
                    {
                        avaliableSprites.Remove(tile[x - 1, y].rend.sprite);
                        avaliableSprites.Remove(tile[x, y - 1].rend.sprite);
                    }
                   tile[x, y].rend.sprite = avaliableSprites[Random.Range(0, avaliableSprites.Count-1)];
                }
            }
        } 
    }

    public void ClearMatch()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (tile[x, y])
                {
                    if (tile[x, y].inMatch == true)
                    {
                        Destroy(tile[x, y]);
                    }
                }
            }
        }
    }

    void ShiftGrid()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (!tile[x, y] && tile[x, y + 1])
                {
                    tile[x, y + 1].transform.position -= new Vector3(0, 1, 0);
                    tile[x, y] = tile[x, y + 1];
                    tile[x, y + 1] = null;
                }
            }
        }
    }

    void CheckVerticalMatches() {
        // Check Vertical Matches
        boardResolved = true;
        for (int x = 0; x < 5; x++) {
            for (int y = 0; y < 3; y++)
            {
                if (SameTileType(tile[x, y], tile[x, y + 1])) {
                    int matched = 1;
                    for (int i = y + 1; i < 5; i++) {
                        if(!SameTileType(tile[x,y], tile[x,i])){
                            break;
                        }
                        matched++;
                    }
                    if(matched >= 3) {
                        for (int i = y; i < y + matched; i++) {
                            tile[x, i].inMatch = true;
                        }
                        boardResolved = false;
                        return;
                    }
                }
            }
        }

    }
    void CheckHorizontalMatches() {
        boardResolved = true;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (SameTileType(tile[x, y], tile[x + 1, y]))
                {
                    int matched = 1;
                    for (int i = x + 1; i < 5; i++)
                    {
                        if (!SameTileType(tile[x, y], tile[i, y]))
                        {
                            break;
                        }
                        matched++;
                    }
                    if (matched >= 3)
                    {
                        for (int i = x; i < x + matched; i++)
                        {
                            tile[i,y].inMatch = true;
                        }
                        boardResolved = false;
                        Debug.Log("Found a horizontal match");
                        return;
                    }
                }
            }
        }
    }

    public void ResolveBoard() {
        boardResolved = false;
        Debug.Log("Resolving board");
        while (!boardResolved)
        {
            //CheckVerticalMatches();
            //CheckHorizontalMatches();
            ClearMatch();
            ShiftGrid();
            FillGrid();
        }
        Debug.Log("Board Resolved");
    }

    bool SameTileType(TileBehaviorVer2 tile1, TileBehaviorVer2 tile2) {
        return tile1.rend.sprite == tile2.rend.sprite;
    }
}
