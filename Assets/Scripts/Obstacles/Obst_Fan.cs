﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obst_Fan : MonoBehaviour {

    public float force;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * force);
        }

    }
}
