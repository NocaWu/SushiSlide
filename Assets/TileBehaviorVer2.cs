using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviorVer2 : MonoBehaviour
{
    public bool inMatch = false;
    private Color selectedColor = new Vector4(1, 1, 1, 0.5f);

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindMatchTiles();
    }

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
