﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obst_Escabeau_Step : MonoBehaviour {

    bool canBeDestroy = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (canBeDestroy)
            Destroy(gameObject);
    }

    public void SetDestroy()
    {
        canBeDestroy = true;
    }
}
