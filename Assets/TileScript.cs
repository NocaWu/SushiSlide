using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private static TileScript prevSelected = null;
    private SpriteRenderer rend;
    private bool isSelected;
    private Color selectedColor = new Vector4(0.5f,0.5f,0.5f,0.5f);

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
                   //Swap(prevSelected.rend);
                   SwapObject(prevSelected.gameObject);
                   prevSelected.Deselect();
               }
           }
    }


    void Swap(SpriteRenderer prev)
    {
        float xDis = transform.position.x - prev.gameObject.transform.position.x;
        float yDis = transform.position.y - prev.gameObject.transform.position.y;

        SpriteRenderer self = gameObject.GetComponent<SpriteRenderer>();

        Sprite temp;

        if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prev.sprite != self.sprite)
        {
            temp = prev.sprite;
            prev.sprite = self.sprite;
            self.sprite = temp;
        }
    }

    void SwapObject(GameObject prev)
    {
        Sprite prevSprite = prev.GetComponent<SpriteRenderer>().sprite;
        Sprite selfSprite = GetComponent<SpriteRenderer>().sprite;
       
        Vector3 tempPos;

        float xDis = transform.position.x - prev.transform.position.x;
        float yDis = transform.position.y - prev.transform.position.y;

        if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prevSprite != selfSprite)
        {
            //tempPos = prev.transform.position;
            //prev.transform.position = transform.position;
            //transform.position = tempPos;
            StartCoroutine(SmoothSwap(prev));
        }
    }

    IEnumerator SmoothSwap(GameObject prev)
    {
        GridManagerScript.instance.isMoving = true;
        Vector3 tempPrev = prev.transform.position;
        Vector3 tempSelf = transform.position;

        for (float f = 0f; f <= 1; f += 0.1f)
        {
            prev.transform.position = Vector3.Lerp(prev.transform.position, tempSelf,f*f);
            transform.position = Vector3.Lerp(transform.position,tempPrev,f*f);
            yield return null;
        }

        GridManagerScript.instance.isMoving = false;
    }
}
