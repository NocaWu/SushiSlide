using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
    public static GameManagerScript instance;
    GameObject selectOne = null;
    GameObject selectTwo = null;
	// Use this for initialization
	void Start () {
        instance = GetComponent<GameManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
