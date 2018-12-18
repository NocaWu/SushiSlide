using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviorVer3 : MonoBehaviour {

    public bool inMatch;
    private bool isSelected;

    private static TileBehaviorVer3 prevSelected = null;

    public SpriteRenderer rend;

    public Color selectedColor = new Vector4(1, 1, 1, 0.5f);

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (!GridManagerVer3.instance.isShifting)
        {
            GridManagerVer3.instance.isShifting = true;

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
                SwapObject();
                prevSelected.Deselect();
            }

            GridManagerVer3.instance.isShifting = false;
        }
    }

    private void Select()
    {
        isSelected = true;
        rend.color = selectedColor;
        prevSelected = this;
    }

    private void Deselect()
    {
        isSelected = false;
        rend.color = Color.white;
        prevSelected = null;
    }

    // swap selected tiles if they're nect to each other
    void SwapObject()
    {
        float xDis = transform.position.x - prevSelected.transform.position.x;
        float yDis = transform.position.y - prevSelected.transform.position.y;

        if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prevSelected.rend.sprite != rend.sprite)
        {
            StartCoroutine(SmoothSwap(prevSelected));
        }
    }

    // after the swap check if there any match, if not, swap it back
    IEnumerator SmoothSwap(TileBehaviorVer3 prev)
    {
        // swap
        Vector3 targetPos = prev.transform.position;
        Vector3 currentPos = transform.position;

        for (float t = 0f; t <= 1; t += 0.1f)
        {
            float lerpAmount = Mathf.SmoothStep(0, 1, t);
            prev.gameObject.transform.position = Vector3.Lerp(targetPos, currentPos, lerpAmount);
            transform.position = Vector3.Lerp(currentPos, targetPos, lerpAmount);
            yield return null;
        }

        prev.gameObject.transform.position = currentPos;
        transform.position = targetPos;

        // tell GridMangager its swap
        GridManagerVer3.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = this;
        GridManagerVer3.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = prev;

        // GridManager check if there's a match in the grid
        GridManagerVer3.instance.CheckGrid();

        // if not, swap them back
        if (!GridManagerVer3.instance.hasMatch)
        {
            for (float t = 0f; t <= 1; t += 0.1f)
            {
                float lerpAmount = Mathf.SmoothStep(0, 1, t);
                prev.transform.position = Vector3.Lerp(currentPos, targetPos, lerpAmount);
                transform.position = Vector3.Lerp(targetPos, currentPos, lerpAmount);
                yield return null;
            }

            prev.gameObject.transform.position = targetPos;
            transform.position = currentPos;

            GridManagerVer3.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = prev;
            GridManagerVer3.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = this;
        }
    }

    // find neighbour tiles, check if the sprite is the same as its own
    // if they are, call it a match
    // if more than 2 in the neighbours are matched, turn inMatch to true
    public void CheckMatch()
    {
        List<TileBehaviorVer3> neighbourTiles = new List<TileBehaviorVer3>();

        int xPos = Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.y);

        if (yPos <= 3)
        {
            neighbourTiles.Add(GridManagerVer3.instance.tile[xPos, yPos + 1]);
        }
        if (yPos >= 1)
        {
            neighbourTiles.Add(GridManagerVer3.instance.tile[xPos, yPos - 1]);
        }
        if (xPos <= 3)
        {
            neighbourTiles.Add(GridManagerVer3.instance.tile[xPos + 1, yPos]);
        }
        if (xPos >= 1)
        {
            neighbourTiles.Add(GridManagerVer3.instance.tile[xPos - 1, yPos]);
        }

        List<TileBehaviorVer3> matchTiles = new List<TileBehaviorVer3>();

        for (int i = 0; i < neighbourTiles.Count; i++)
        {
            if (neighbourTiles[i] && neighbourTiles[i].rend.sprite == rend.sprite)
            {
                matchTiles.Add(neighbourTiles[i]);
            }
        }

        if (matchTiles.Count >= 2)
        {
            inMatch = true;
            GridManagerVer3.instance.hasMatch = true;

            for (int i = 0; i < matchTiles.Count; i++)
            {
                matchTiles[i].inMatch = true;
            }
        }
    }
}
