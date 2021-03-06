﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public GameObject player;       //Public variable to store a reference to the player game object
    public GameObject maxRight;
    public GameObject maxLeft;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    // Use this for initialization
    void Start () {

        // Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        Camera cam = GetComponent<Camera>();
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        if (player.transform.position.x >= (transform.position.x + (width / 4)))
        {
            Vector3 newPosition = transform.position;
            newPosition.x = player.transform.position.x + offset.x - (width / 4);
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }

        if (player.transform.position.x <= (transform.position.x - (width / 4)))
        {
            Vector3 newPosition = transform.position;
            newPosition.x = player.transform.position.x + offset.x + (width / 4);
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }

    }
}
