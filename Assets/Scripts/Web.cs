﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour {

    private string spawnerName;
    private AudioSource sound;

	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if(other.tag == "Player" && other.name != spawnerName)
        {
            sound.Play();
            other.GetComponent<PlayableHero>().Snare();

            Destroy(gameObject);
        }
    }
    
    public void SetSpawnerName(string spawnerName)
    {
        this.spawnerName = spawnerName;
    }
}
