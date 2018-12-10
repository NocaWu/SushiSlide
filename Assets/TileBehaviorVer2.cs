using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviorVer2 : MonoBehaviour
{
    public bool inMatch = false;

    private static TileBehaviorVer2 prevSelected = null;
    private SpriteRenderer rend;
    private bool isSelected;
    private Color selectedColor = new Vector4(1, 1, 1, 0.5f);

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FindMatchTiles();
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
                SwapObject(transform.position, prevSelected.gameObject.transform.position, prevSelected.gameObject);
                prevSelected.Deselect();
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
        GridManagerVer2.instance.isShifting = true;

        Sprite prevSprite = prev.GetComponent<SpriteRenderer>().sprite;
        Sprite selfSprite = GetComponent<SpriteRenderer>().sprite;

        float xDis = transform.position.x - prev.transform.position.x;
        float yDis = transform.position.y - prev.transform.position.y;

        if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prevSprite != selfSprite)
        {
            StartCoroutine(SmoothSwap(currentPos, targetPos, prev));
        }

        GridManagerVer2.instance.isShifting = false;
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
        GridManagerVer2.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = gameObject;
        GridManagerVer2.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = prev;

        FindMatchTiles();
        if (inMatch == false)
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
            GridManagerVer2.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = prev;
            GridManagerVer2.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = gameObject;
        }
    }

    // check the surroundings and put the match tiles in a list
    // change all match tile's inMatch bool to true
    void FindMatchTiles()
    {
        List<GameObject> matchTiles = new List<GameObject>();
        List<GameObject> neighbourTiles = new List<GameObject>();

        int xPos = Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.y);

        if (yPos <= 3)
        {
            GameObject tileAbove = GridManagerVer2.instance.tile[xPos, yPos + 1];
            neighbourTiles.Add(tileAbove);
        }
        if (yPos >= 1)
        {
            GameObject tileBelow = GridManagerVer2.instance.tile[xPos, yPos - 1];
            neighbourTiles.Add(tileBelow);
        }
        if (xPos <= 3)
        {
            GameObject tileToright = GridManagerVer2.instance.tile[xPos + 1, yPos];
            neighbourTiles.Add(tileToright);
        }
        if (xPos >= 1)
        {
            GameObject tileToleft = GridManagerVer2.instance.tile[xPos - 1, yPos];
            neighbourTiles.Add(tileToleft);
        }

        for (int i = 0; i < neighbourTiles.Count; i++)
        {
            if (neighbourTiles[i])
            {
                if (neighbourTiles[i].GetComponent<SpriteRenderer>().sprite == gameObject.GetComponent<SpriteRenderer>().sprite)
                {
                    matchTiles.Add(neighbourTiles[i]);
                }
            }
        }

        if (matchTiles.Count >= 2)
        {
            inMatch = true;
            for (int i = 0; i < matchTiles.Count; i++)
            {
                matchTiles[i].GetComponent<TileBehaviorVer2>().inMatch = true;
            }
        }
    }
}
