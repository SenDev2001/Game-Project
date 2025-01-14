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

    // Order of road prefabs to be spawned
    private int[] prefabOrder = { 0, 1, 2, 3 };
    private int currentPrefabIndex = 0;

    // Queue to manage active roads
    private Queue<GameObject> activeRoads = new Queue<GameObject>();

    private void Start()
    {
        // Find the player object by its "Player" tag and get its transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Spawn the initial roads on the screen
        SpawnInitialRoads();
    }

    private void Update()
    {
        // Check if the player has passed the second road in the active roads queue
        CheckRoadsAndSpawnNew();
    }

    // spawn the initial roads on the screen
    private void SpawnInitialRoads()
    {
        for (int i = 0; i < amnRoadOnScreen; i++)
        {
            SpawnRoad();
        }
    }

    //  check roads and spawn a new one when needed
    private void CheckRoadsAndSpawnNew()
    {
        if (activeRoads.Count > 1)
        {
            GameObject secondRoad = activeRoads.ToArray()[1];
            if (playerTransform.position.z > secondRoad.transform.position.z + roadLength)
            {
                DestroyFirstRoadAndSpawnNew();
            }
        }
    }

    // destroy the first road and spawn a new one
    private void DestroyFirstRoadAndSpawnNew()
    {
        GameObject firstRoad = activeRoads.Dequeue();
        Destroy(firstRoad);
        SpawnRoad();
    }

    // spawn a new road at the current spawn position
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

    //spawn snow above the road
    private void SpawnSnowAboveRoad(GameObject road)
    {
        Vector3 snowPosition = road.transform.position + new Vector3(0, 10, 0);
        GameObject snow = Instantiate(snowPrefab, snowPosition, Quaternion.identity);

        // Set snow as a child of the road so it moves with the road
        snow.transform.SetParent(road.transform);
    }
}
