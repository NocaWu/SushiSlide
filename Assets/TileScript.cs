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
	}

    private void Deselect()
    {
        isSelected = false;
        rend.color = Color.white;
        prevSelected.rend.color = Color.white;
        prevSelected = null;
    }

	void OnMouseDown()
	{
        if(/*rend.sprite && */!GridManagerScript.instance.isMoving)
        {
            if (isSelected == true)
            {
                Deselect();
            }
            else if (prevSelected == null)
            {
                prevSelected = gameObject.GetComponent<TileScript>();
                Select();
            } 
            else 
            {
                //Swap(prevSelected.rend);
                SwapObject(prevSelected.gameObject);
                Deselect();
            }
        }
	}

    void Swap(SpriteRenderer prev)
    {
        float xDis = gameObject.transform.position.x - prev.gameObject.transform.position.x;
        float yDis = gameObject.transform.position.y - prev.gameObject.transform.position.y;
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
       //Sprite prevSprite = prev.GetComponent<SpriteRenderer>().sprite;
        //Sprite selfSprite = GetComponent<SpriteRenderer>().sprite;
        Vector3 selfPos = transform.position;
        Vector3 prevPos = prev.transform.position;
        Vector3 tempPos;
        //float xDis = selfPos.x - prevPos.x;
        //float yDis = selfPos.y - prevPos.y;

        //if (Mathf.Abs(xDis) < 1.1f && Mathf.Abs(yDis) < 1.1f && prevSprite != selfSprite)
       // {
            tempPos = prevPos;
            prevPos = selfPos;
            selfPos = tempPos;
        //}
    }
}
