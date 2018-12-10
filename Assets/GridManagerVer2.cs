using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerVer2 : MonoBehaviour {

    public static GridManagerVer2 instance;

    public GameObject tilePrefab;
    public GameObject[,] tile = new GameObject[5, 5]; // the whole grid(5*5) tiles as an array

    public Sprite[] tileSprite;

    public bool isShifting;

    // Use this for initialization
    void Awake()
    {
        instance = GetComponent<GridManagerVer2>();
        FillGrid();
    }

	void Update()
	{
        ClearMatch();
        ShiftGrid();
        FillGrid();
	}

    void FillGrid()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!tile[x, y])
                {
                    tile[x, y] = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile[x, y].transform.parent = gameObject.transform;
                    tile[x, y].GetComponent<SpriteRenderer>().sprite = tileSprite[Random.Range(0, tileSprite.Length)];
                }
            }
        } 
    }

    void ClearMatch()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (tile[x, y] && tile[x, y].GetComponent<TileBehaviorVer2>().inMatch == true)
                {
                    Destroy(tile[x, y]);
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
}
