using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private static TileScript prevSelected = null;
    private SpriteRenderer rend;
    private bool isSelected;
    private Color selectedColor = new Vector4(1f,1f,1f,0.5f);

	void Start()
	{
        rend = GetComponent<SpriteRenderer>();
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
                    GridManagerScript.instance.isMoving = true;
                    Vector3 tempSelf = transform.position;
                    Vector3 tempPrev = prevSelected.gameObject.transform.position;
                    SwapObject(transform.position,prevSelected.gameObject.transform.position, prevSelected.gameObject);

                    if(hasMatch())
                    {
                        // clear matches
                    }
                    else
                    {
                        SwapObject(transform.position, tempSelf, prevSelected.gameObject);
                    }

                    GridManagerScript.instance.isMoving = false;

                    prevSelected.Deselect();
               }
           }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }

    // swap selected tiles
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
    IEnumerator SmoothSwap(Vector3 currentPos, Vector3 targetPos, GameObject prev)
    {
        for (float t = 0f; t <= 1; t += 0.1f)
        {
            float lerpAmount = Mathf.SmoothStep(0,1,t);
            prev.transform.position = Vector3.Lerp(prev.transform.position, currentPos,lerpAmount);
            transform.position = Vector3.Lerp(transform.position,targetPos,lerpAmount);
            yield return null;
        }

        prev.transform.position = currentPos;
        transform.position = targetPos;
    }

    private bool hasMatch()
    {
        return false;
    }
}
