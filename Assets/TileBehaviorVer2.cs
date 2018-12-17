using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviorVer2 : MonoBehaviour
{
    public bool inMatch = false;

    private static TileBehaviorVer2 prevSelected = null;
    public SpriteRenderer rend;
    private bool isSelected;
    private Color selectedColor = new Vector4(1, 1, 1, 0.5f);

    // Use this for initialization
    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        MarkMatchTiles();
    }

	void Update()
	{
         //bug seems related to "FindMatchTiles"
         //where is the right time to put it...
        //if(!GridManagerVer2.instance.isShifting)
        //{
        //    GridManagerVer2.instance.isShifting = true;
        //    MarkMatchTiles();
        //    GridManagerVer2.instance.isShifting = false;
        //}
	}

	void OnMouseDown()
    {
        if (rend.sprite && !GridManagerVer2.instance.isShifting)
        {
            if (isSelected == true)
            {
                Deselect();
            }
            else if (prevSelected == null)
            {
                Select();
            }
            else
            {
                GridManagerVer2.instance.isShifting = true;
                SwapObject(transform.position, prevSelected.gameObject.transform.position, prevSelected.gameObject);
                prevSelected.Deselect();
                //GridManagerVer2.instance.ResolveBoard();
                GridManagerVer2.instance.isShifting = false;
            }
        }
    }

    private void Select()
    {
        isSelected = true;
        rend.color = selectedColor;
        prevSelected = gameObject.GetComponent<TileBehaviorVer2>();
    }

    private void Deselect()
    {
        isSelected = false;
        rend.color = Color.white;
        prevSelected = null;
    }

    // swap selected tiles if they're nect to each other
    void SwapObject(Vector3 currentPos, Vector3 targetPos, GameObject prev)
    {
        Sprite prevSprite = prev.GetComponent<SpriteRenderer>().sprite;
        Sprite selfSprite = GetComponent<SpriteRenderer>().sprite;

        float xDis = transform.position.x - prev.transform.position.x;
        float yDis = transform.position.y - prev.transform.position.y;

        if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prevSprite != selfSprite)
        {
            StartCoroutine(SmoothSwap(currentPos, targetPos, prev));
        }
    }

    // visual smoothing swaping
    // after the swap check if there any match, if not, swap it back
    IEnumerator SmoothSwap(Vector3 currentPos, Vector3 targetPos, GameObject prev)
    {
        for (float t = 0f; t <= 1; t += 0.1f)
        {
            float lerpAmount = Mathf.SmoothStep(0, 1, t);
            prev.transform.position = Vector3.Lerp(targetPos, currentPos, lerpAmount);
            transform.position = Vector3.Lerp(currentPos, targetPos, lerpAmount);
            yield return null;
        }
        prev.transform.position = currentPos;
        transform.position = targetPos;

        // tell GridMangager its swap
        GridManagerVer2.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = this;
        GridManagerVer2.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = prev.GetComponent<TileBehaviorVer2>();

        MarkMatchTiles();
        prev.GetComponent<TileBehaviorVer2>().MarkMatchTiles();

        if(!inMatch && !prev.GetComponent<TileBehaviorVer2>().inMatch)
        {
            for (float t = 0f; t <= 1; t += 0.1f)
            {
                float lerpAmount = Mathf.SmoothStep(0, 1, t);
                prev.transform.position = Vector3.Lerp(currentPos, targetPos, lerpAmount);
                transform.position = Vector3.Lerp(targetPos, currentPos, lerpAmount);
                yield return null;
            }

            prev.transform.position = targetPos;
            transform.position = currentPos;

            // tell GridMangager its swap
            GridManagerVer2.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = prev.GetComponent<TileBehaviorVer2>();
            GridManagerVer2.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = this;
        }
    }

    void MarkMatchTiles()
    {
        List<TileBehaviorVer2> neighbourTiles = new List<TileBehaviorVer2>();

        int xPos = Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.y);

        if (yPos <= 3)
        {
            TileBehaviorVer2 tileAbove = GridManagerVer2.instance.tile[xPos, yPos + 1];
            neighbourTiles.Add(tileAbove);
        }
        if (yPos >= 1)
        {
            TileBehaviorVer2 tileBelow = GridManagerVer2.instance.tile[xPos, yPos - 1];
            neighbourTiles.Add(tileBelow);
        }
        if (xPos <= 3)
        {
            TileBehaviorVer2 tileToright = GridManagerVer2.instance.tile[xPos + 1, yPos];
            neighbourTiles.Add(tileToright);
        }
        if (xPos >= 1)
        {
            TileBehaviorVer2 tileToleft = GridManagerVer2.instance.tile[xPos - 1, yPos];
            neighbourTiles.Add(tileToleft);
        }

        List<TileBehaviorVer2> matchTiles = new List<TileBehaviorVer2>();

        // how to use while loop to search all matched neighbours?
        for (int i = 0; i < neighbourTiles.Count; i++)
        {
            if (neighbourTiles[i].rend.sprite == rend.sprite)
            {
                matchTiles.Add(neighbourTiles[i]);
            }
        }

        if (matchTiles.Count >= 2)
        {
            inMatch = true;
            for (int i = 0; i < matchTiles.Count; i++)
            {
                matchTiles[i].inMatch = true;
            }
        }
    }
}
