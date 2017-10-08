﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverPizza : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject generator = GameObject.FindGameObjectWithTag("Generator");
            generator.GetComponent<GenerateMap>().GetUp(other.GetComponent<PlayableHero>().currentPlayer);
            
        }
    }
}
