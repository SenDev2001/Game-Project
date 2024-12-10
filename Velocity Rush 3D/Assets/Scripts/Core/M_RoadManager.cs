using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs;
    public GameObject snowPrefab;
    private Transform playerTransform;

    private float spawnZ = 20f;
    private float roadLength = 50f;
    private int amnRoadOnScreen = 4;

    private int[] prefabOrder = { 0, 1, 2, 3 };
    private int currentPrefabIndex = 0;

    private Queue<GameObject> activeRoads = new Queue<GameObject>();

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
        if (activeRoads.Count > 1)
        {
            GameObject secondRoad = activeRoads.ToArray() [1];
            if (playerTransform.position.z > secondRoad.transform.position.z + roadLength)
            { 
                GameObject firstRoad = activeRoads.Dequeue();  
                Destroy(firstRoad);
                SpawnRoad();
            }
        }
    }

    private void SpawnRoad()
    {
        int prefabIndex = prefabOrder[currentPrefabIndex];
        GameObject road = Instantiate(roadPrefabs[prefabIndex]);

        road.transform.SetParent(transform);
        road.transform.position = new Vector3(0, 0, spawnZ);

        SpawnSnowAboveRoad(road);

        activeRoads.Enqueue(road);

        spawnZ += roadLength;
        currentPrefabIndex = (currentPrefabIndex + 1) % prefabOrder.Length;
    }

    private void SpawnSnowAboveRoad(GameObject road)
    {
      
        Vector3 snowPosition = road.transform.position + new Vector3(0, 10, 0); 
        GameObject snow = Instantiate(snowPrefab, snowPosition, Quaternion.identity);

        
        snow.transform.SetParent(road.transform);
    }
}
