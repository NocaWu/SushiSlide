using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerVer3 : MonoBehaviour {
   
    public static GridManagerVer3 instance;

    public GameObject tilePrefab;
    public TileBehaviorVer3[,] tile = new TileBehaviorVer3[5, 5]; // the whole grid(5*5) tiles as an array
    TileBehaviorVer3[,] toMove = new TileBehaviorVer3[5, 5];

    public Sprite[] tileSprite;

    public bool isShifting = false;
    public bool hasMatch;
	
    // Use this for initialization
    void Awake()
    {
        instance = this;
        FillGrid();
        CheckGrid();
        ResolveGrid();
    }
	
	// Update is called once per frame
	void LateUpdate () 
    {
        ResolveGrid();
	}

    void ResolveGrid()
    {
        if (!isShifting)
        {
            isShifting = true;

            while (hasMatch)
            {
                ClearMatch();
                ShiftGrid();
                CheckGrid();
            }
            isShifting = false;
        }
    }

    public void CheckGrid()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if(tile[x, y])
                {
                    tile[x, y].CheckMatch(); 
                }
            }
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
                    tile[x, y] = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<TileBehaviorVer3>();
                    tile[x, y].transform.parent = gameObject.transform;
                    List<Sprite> avaliableSprites = new List<Sprite>();
                    avaliableSprites.AddRange(tileSprite);
                    if (x > 0 && y > 0)
                    {
                        avaliableSprites.Remove(tile[x - 1, y].rend.sprite);
                        avaliableSprites.Remove(tile[x, y - 1].rend.sprite);
                    }
                    tile[x, y].rend.sprite = avaliableSprites[Random.Range(0, avaliableSprites.Count)];
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
                if (tile[x, y])
                {
                    if (tile[x, y].inMatch == true)
                    {
                        Destroy(tile[x, y].gameObject);
                        tile[x, y] = null;
                    }
                }
            }
        }

        hasMatch = false;
    }

    // add a bool for shift grid
    void ShiftGrid()
    {
        for (int y = 0; y <4; y++)
        {
            for (int x = 0; x <5; x++)
            {
                if (tile[x, y]==null)
                {
                    for (int n = 1; n < 4; n++)
                    {
                        if (y+n <= 4 && tile[x, y + n] != null)
                        {
                            toMove[x,y+n] = tile[x, y + n];

                            StartCoroutine(ShiftTileDown(toMove[x, y + n],
                                                         toMove[x, y + n].transform.position,
                                                         new Vector3(0, n, 0)));
                            tile[x, y] = toMove[x, y + n];
                            tile[x, y + n] = null;

                            break;
                        }
                    }
                }
            }
        }
        //FillGrid();
    }

    IEnumerator ShiftTileDown(TileBehaviorVer3 toBeMoved, Vector3 currentPos, Vector3 moveValue)
    {
        Vector3 targetPos = currentPos - moveValue;
        for (float t = 0f; t <= 1; t += 0.1f)
        {
            float lerpAmount = Mathf.SmoothStep(0, 1, t);
            toBeMoved.transform.position = Vector3.Lerp(currentPos, targetPos, lerpAmount);
            yield return null;
        }

        toBeMoved.transform.position = targetPos;
    }
}
