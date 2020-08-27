using System.Collections;
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
            for (int i = 0; i < EnemyToSpawnAmount; )
            {
                //set a random spawn index
                spawnPointIndex = Random.Range(0, spawnPoints.Length);

                //set a random enemy index
                enemyIndex = Random.Range(0, enemyPrefab.Length);

                //get the place where enemy will spawn at
                Transform spawnPosition = spawnPoints[spawnPointIndex];

                Vector3 spawnRandomOffset = new Vector3(Random.Range(-50, 50), 100, Random.Range(-50, 50));
                spawnPosition.position += spawnRandomOffset;

                RaycastHit hitInfo;
                //check Position To Spawn, raycast hit the ground below then allow spawn
                if (Physics.Raycast(spawnPosition.position, Vector3.down, out hitInfo, Mathf.Infinity))
                {
                    //check hit tree or not
                    float terrainPosY = Terrain.activeTerrain.SampleHeight(transform.position);
                    if ((hitInfo.point.y - terrainPosY) < 5)
                    {
                        spawnPosition.position = new Vector3(spawnPosition.position.x, hitInfo.point.y, spawnPosition.position.z);
                        //spawn enemy
                        //Instantiate(enemyPrefab[enemyIndex], spawnPosition.position, spawnPosition.rotation);
                        PhotonNetwork.InstantiateRoomObject(enemyPrefab[enemyIndex].name, spawnPosition.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up), 0);
                        i++;
                    }
                }

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
                PhotonNetwork.InstantiateRoomObject(bossPrefab.name, spawnPoints[0].position, Quaternion.identity, 0);
            }
        }

    }
}
