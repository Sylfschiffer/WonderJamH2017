﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerThrowPizza : MonoBehaviour {

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
            GetComponentInParent<PizzaThrow>().GoToPlayer(other.transform);
            Destroy(gameObject);
        }
    }
}
