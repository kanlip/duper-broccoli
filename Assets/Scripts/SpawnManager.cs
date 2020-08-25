﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameObject [] enemyPrefab;
    public Transform [] spawnPoints;
    public GameObject bossPrefab;

    public int EnemyToSpawnAmount = 10;

    private int spawnPointIndex;
    private int enemyIndex;
    private int counter;
    private GameObject [] enemyCount;

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            //spawn 10 random enemy small or med
            for (int i = 0; i < EnemyToSpawnAmount; i++)
            {
                //set a random spawn index
                spawnPointIndex = Random.Range(0, spawnPoints.Length);

                //set a random enemy index
                enemyIndex = Random.Range(0, enemyPrefab.Length);

                //get the place where enemy will spawn at
                Transform spawnPosition = spawnPoints[spawnPointIndex];


                Vector3 spawnRandomOffset = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));

                //spawn enemy
                //Instantiate(enemyPrefab[enemyIndex], spawnPosition.position, spawnPosition.rotation);
                PhotonNetwork.Instantiate(enemyPrefab[enemyIndex].name, spawnPosition.position+ spawnRandomOffset, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up), 0);
            }

            //find all the gameobject tag with enemy
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy");

            counter = enemyCount.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //testing if the boss will spawn if all enemy destroy
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Destory all the enemy
            Destroy(enemyCount[counter-1]);
            counter--;
            
            //if not more enemy left boss will spawn
            if (counter == 0)
            {
                //Instantiate(bossPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
                PhotonNetwork.Instantiate(bossPrefab.name, spawnPoints[0].position, Quaternion.identity, 0);
            }
        }

    }
}
