using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScriptVerTwo : MonoBehaviour {
    private bool isSelected = false;
	// Use this for initialization
	void Start () {
		
	}

	private void OnMouseDown()
	{
        if(isSelected == false)
        {
            isSelected = true;
        }
	}
}
