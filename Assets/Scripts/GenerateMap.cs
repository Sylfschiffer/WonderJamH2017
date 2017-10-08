﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour {
    public float baseX;
    public float baseY;
    public int towerSize;
    public int numberOfFloor;
	// Use this for initialization
	void Start () {
        float height = 2f * Camera.main.orthographicSize;

        for (int i = 0; i < towerSize;i++)
        {
            int randomFloor = Random.Range(1, numberOfFloor + 1);
            GameObject newFloor = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Levels/Level" + randomFloor));
            newFloor.transform.position = new Vector3(baseX, baseY+i*height, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
