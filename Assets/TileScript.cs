using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private static TileScript prevSelected = null;
    private SpriteRenderer rend;
    private bool isSelected;
    private Color selectedColor = new Vector4(1f,1f,1f,0.5f);
    private bool hasMatch = false;

	void Start()
	{
        rend = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
        //if(!hasBelow())
        //{
        //    transform.position -= new Vector3(0, 1, 0);
        //    GridManagerScript.instance.tile[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)] = gameObject;
        //} 
        //else if (Match().Count > 1)
        //{
        //    ClearMatch();
        //}
	}

	private void Select()
	{
        isSelected = true;
        rend.color = selectedColor;
        prevSelected = gameObject.GetComponent<TileScript>();
	}

    private void Deselect()
    {
        isSelected = false;
        rend.color = Color.white;
        prevSelected = null;
    }

    void OnMouseDown()
    {
           if(rend.sprite && !GridManagerScript.instance.isMoving)
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
                    SwapObject(transform.position,prevSelected.gameObject.transform.position, prevSelected.gameObject);
                    prevSelected.Deselect();
               }
           }
    }

    // swap selected tiles if they're nect to each other
    void SwapObject(Vector3 currentPos, Vector3 targetPos, GameObject prev)
    {
        GridManagerScript.instance.isMoving = true;

        Sprite prevSprite = prev.GetComponent<SpriteRenderer>().sprite;
        Sprite selfSprite = GetComponent<SpriteRenderer>().sprite;
       
        float xDis = transform.position.x - prev.transform.position.x;
        float yDis = transform.position.y - prev.transform.position.y;

        if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prevSprite != selfSprite)
        {
            StartCoroutine(SmoothSwap(currentPos, targetPos, prev));
        }

        GridManagerScript.instance.isMoving = false;
    }

    // visual smoothing swaping
    // after the swap check if there any match, if not, swap it back
    IEnumerator SmoothSwap(Vector3 currentPos, Vector3 targetPos, GameObject prev)
    {
        for (float t = 0f; t <= 1; t += 0.1f)
        {
            float lerpAmount = Mathf.SmoothStep(0,1,t);
            prev.transform.position = Vector3.Lerp(targetPos, currentPos,lerpAmount);
            transform.position = Vector3.Lerp(currentPos,targetPos,lerpAmount);
            yield return null;
        }

        prev.transform.position = currentPos;
        transform.position = targetPos;

        // tell GridMangager its swap
        GridManagerScript.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = gameObject;
        GridManagerScript.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = prev;

        if (Match().Count <= 1)
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
            GridManagerScript.instance.tile[Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y)] = prev;
            GridManagerScript.instance.tile[Mathf.RoundToInt(currentPos.x), Mathf.RoundToInt(currentPos.y)] = gameObject;
        }
        //else 
        //{
        //    ClearMatch();
        //}
    }

    private bool hasBelow()
    {
        GameObject below = null;

        if (Mathf.RoundToInt(transform.position.y) - 1 >= 0)
        {
            below = GridManagerScript.instance.tile[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) - 1];
        }

        if (below == null && Mathf.RoundToInt(transform.position.y) > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private List<GameObject> Match()
    {
        List<GameObject> MatchTiles = new List<GameObject>();
        List<GameObject> AdjuctTiles = new List<GameObject>();

        if (hasBelow() && Mathf.RoundToInt(transform.position.y) - 1 >= 0)
        {
            GameObject below = GridManagerScript.instance.tile[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) - 1];
            AdjuctTiles.Add(below);
        }

        if(Mathf.RoundToInt(transform.position.y) + 1 <= 4)
        {
            GameObject above = GridManagerScript.instance.tile[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y) + 1];
            AdjuctTiles.Add(above);
        }

        if(Mathf.RoundToInt(transform.position.x) + 1 <= 4)
        {
            GameObject toRight = GridManagerScript.instance.tile[Mathf.RoundToInt(transform.position.x) + 1, Mathf.RoundToInt(transform.position.y)];
            AdjuctTiles.Add(toRight);
        }

        if(Mathf.RoundToInt(transform.position.x) - 1 >= 0)
        {
            GameObject toLeft = GridManagerScript.instance.tile[Mathf.RoundToInt(transform.position.x) - 1, Mathf.RoundToInt(transform.position.y)];
            AdjuctTiles.Add(toLeft);
        }

        for (int i = 0; i < AdjuctTiles.Count; i++)
        {
            if(AdjuctTiles[i].GetComponent<SpriteRenderer>().sprite == rend.sprite)
            {
                MatchTiles.Add(AdjuctTiles[i]);
            }
        }
        return MatchTiles;
    }

    private void ClearMatch()
    {
        // clear matches
        // move down
        // instantiate new tiles if empty
        //hasMatch = false;
        for (int i = 0; i < Match().Count; i++)
        {
            Destroy(Match()[i]);
        }
        Destroy(gameObject);
    }
}
