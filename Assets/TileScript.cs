using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    //public static TileScript selectedTile;
    private SpriteRenderer rend;
    private bool isSelected;
    private Color selectedColor = new Vector4(0.5f,0.5f,0.5f,0.5f);

	void Start()
	{
        rend = GetComponent<SpriteRenderer>();
	}

    void OnMouseDown()
	{
        if(rend.sprite && !GridManagerScript.instance.isMoving)
        {
            if (isSelected == true)
            {
                isSelected = false;
                rend.color = Color.white;
            }
            else
            {
                isSelected = true;
                rend.color = selectedColor;
            } 
        }
	}
}
