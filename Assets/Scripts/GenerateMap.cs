﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerateMap : MonoBehaviour {
    public float baseX;
    public float baseY;
    public int towerSize;
    public int numberOfFloor;
    public GameObject[] tableFloor;
    public GameObject player1, player2;
    public static int actualFloor = 0;
    public float test = 0f;
    public GameObject brickWall;
    public float targetImagey;
    float height;
    float tweak = 1;
    bool allRandom = false;
    bool[] floors;
    AudioSource audio;
    // Use this for initialization
    void Start () {
        towerSize = CharacterSelect.nombreEtages;
        if (towerSize < numberOfFloor)
        {
            allRandom = true;
            floors = new bool[numberOfFloor];
        }
        actualFloor = 0;
        height = 2f * Camera.main.orthographicSize;
        tableFloor = new GameObject[towerSize];
        audio = GetComponent<AudioSource>();
        

        for (int i = 0; i < towerSize;i++)
        {
            int randomFloor = Random.Range(1, numberOfFloor + 1);
            if (allRandom)
            {
                while (floors[randomFloor - 1])
                {
                    randomFloor = Random.Range(1, numberOfFloor + 1);
                }
                floors[randomFloor - 1] = true;
            }
            GameObject newFloor = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Levels/Level" + randomFloor));
            newFloor.transform.position = new Vector3(baseX, baseY+i*height, 0);
            tableFloor[i] = newFloor;
        }


        switch (CharacterSelect.player1Char)
        {
            case CharacterSelect.Characters.Batcroute:
                player1 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_Batman"));
                player1.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player1;
                player1.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player1.transform.position = player1.GetComponent<PlayableHero>().spawn.position;
                break;

            case CharacterSelect.Characters.JeanGras:
                player1 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_Jean"));
                player1.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player1;
                player1.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player1.transform.position = player1.GetComponent<PlayableHero>().spawn.position;
                break;

            case CharacterSelect.Characters.Sauceman:
                player1 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_Aquaman"));
                player1.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player1;
                player1.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player1.transform.position = player1.GetComponent<PlayableHero>().spawn.position;
                break;

            case CharacterSelect.Characters.Spidercheese:
                player1 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_SpiderMan"));
                player1.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player1;
                player1.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player1.transform.position = player1.GetComponent<PlayableHero>().spawn.position;
                break;
        }
        player1.GetComponent<SpriteRenderer>().sortingOrder = 2;
        player1.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
        player1.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2;


        switch (CharacterSelect.player2Char)
        {
            case CharacterSelect.Characters.Batcroute:
                player2 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_Batman"));
                player2.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player2;
                player2.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player2.transform.position = player2.GetComponent<PlayableHero>().spawn.position;
                break;

            case CharacterSelect.Characters.JeanGras:
                player2 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_Jean"));
                player2.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player2;
                player2.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player2.transform.position = player2.GetComponent<PlayableHero>().spawn.position;
                break;

            case CharacterSelect.Characters.Sauceman:
                player2 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_Aquaman"));
                player2.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player2;
                player2.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player2.transform.position = player2.GetComponent<PlayableHero>().spawn.position;
                break;

            case CharacterSelect.Characters.Spidercheese:
                player2 = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Characters/Char_SpiderMan"));
                player2.GetComponent<PlayableHero>().currentPlayer = CurrentPlayer.Player2;
                player2.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
                player2.transform.position = player2.GetComponent<PlayableHero>().spawn.position;
                break;
        }
        player2.GetComponent<SpriteRenderer>().sortingOrder = 3;
        player2.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
        player2.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 3;

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetUp(CurrentPlayer.Player1);
        }
		if(test > 0)
        {
            brickWall.SetActive(true);
            test -= Time.deltaTime;
            float y = Mathf.MoveTowards(brickWall.transform.position.y, targetImagey, 25f);
            brickWall.transform.position = new Vector3(0, y, 0);
        }
        else
        {
            brickWall.transform.position = new Vector3(0, 0, 0);
            brickWall.SetActive(false);
        }
	}

    public void GetUp(CurrentPlayer actualPlayer)
    {
        actualFloor++;
        Debug.Log(tableFloor.Length);
        if (actualFloor+1 > tableFloor.Length)
        {
            tweak = 3;
        }
        if (actualPlayer == CurrentPlayer.Player1)
        {
            Money.moneyP1 += 100.00f * tweak;
        }
        else
        {
            Money.moneyP2 += 100.00f * tweak;
        }
        if (tweak == 3)
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            audio.Play();
            player1.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
            player1.transform.position = player1.GetComponent<PlayableHero>().spawn.position;
            player1.GetComponent<PlayableHero>().ChangeState(new Idle(player1.GetComponent<PlayableHero>()));
            player1.GetComponent<PlayableHero>().powerDelay = 0;
            player1.GetComponent<PlayableHero>().cptPowerInLevel = 0;
            player1.GetComponent<PlayableHero>().powerUsed = false;
            player2.GetComponent<PlayableHero>().spawn = tableFloor[actualFloor].transform.GetChild(1);
            player2.transform.position = player1.GetComponent<PlayableHero>().spawn.position;
            player2.GetComponent<PlayableHero>().ChangeState(new Idle(player2.GetComponent<PlayableHero>()));
            player2.GetComponent<PlayableHero>().powerDelay = 0;
            player2.GetComponent<PlayableHero>().cptPowerInLevel = 0;
            player2.GetComponent<PlayableHero>().powerUsed = false;
            Camera.main.transform.Translate(Vector3.up * height);
            test = 3f;
        }
        
    }

}
