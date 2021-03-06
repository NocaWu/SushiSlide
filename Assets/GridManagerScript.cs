﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerScript : MonoBehaviour {

    public static GridManagerScript instance;

    public GameObject tilePrefab;
    public GameObject[,] tile= new GameObject[5,5]; // the whole grid(5*5) tiles as an array

    public Sprite[] tileSprite;

    public bool isMoving;

	// Use this for initialization
	void Start () {
        instance = GetComponent<GridManagerScript>();

        // spawn a 5 * 5 grid
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                tile[x,y] = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile[x,y].transform.parent = gameObject.transform;
                tile[x,y].GetComponent<SpriteRenderer>().sprite = tileSprite[Random.Range(0,tileSprite.Length)];
            }
        }
	}

	void Update()
	{
        //for (int x = 0; x < 5; x++)
        //{
        //    if (!tile[x,4])
        //    {
        //        tile[x, 4] = Instantiate(tilePrefab, new Vector3(x, 4, 0), Quaternion.identity);
        //        tile[x, 4].transform.parent = gameObject.transform;
        //        tile[x, 4].GetComponent<SpriteRenderer>().sprite = tileSprite[Random.Range(0, tileSprite.Length)];
        //    }
        //}
	}
}
