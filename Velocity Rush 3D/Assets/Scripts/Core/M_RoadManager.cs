using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs;

    private Transform playerTransform;

    private float spawnZ = 20f;
    private float roadLength = 50f;
    private int amnRoadOnScreen = 5;

   
    private int[] prefabOrder = { 0, 1, 2, 3 }; 
    private int currentPrefabIndex = 0;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        
        for (int i = 0; i < amnRoadOnScreen; i++)
        {
            SpawnRoad();
        }
    }

    private void Update()
    {
       
        if (playerTransform.position.z > (spawnZ - (amnRoadOnScreen * roadLength)))
        {
            SpawnRoad();
        }
    }

    private void SpawnRoad()
    {
       
        int prefabIndex = prefabOrder[currentPrefabIndex];
        GameObject go = Instantiate(roadPrefabs[prefabIndex]) as GameObject;


        go.transform.SetParent(transform);
        go.transform.position = new Vector3(0, 0, spawnZ);

        spawnZ += roadLength;

        currentPrefabIndex = (currentPrefabIndex + 1) % prefabOrder.Length;
    }
}

