using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs;  // Array to hold different road prefabs
    public GameObject snowPrefab;  // Snow particle system prefab
    private Transform playerTransform;

    private float spawnZ = 20f;
    private float roadLength = 50f;
    private int amnRoadOnScreen = 5;

    private int[] prefabOrder = { 0, 1, 2, 3 };
    private int currentPrefabIndex = 0;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Initially spawn roads and their snow particles
        for (int i = 0; i < amnRoadOnScreen; i++)
        {
            SpawnRoad();
        }
    }

    private void Update()
    {
        // Check if the player has crossed the threshold to spawn a new road
        if (playerTransform.position.z > (spawnZ - (amnRoadOnScreen * roadLength)))
        {
            SpawnRoad();
        }
    }

    private void SpawnRoad()
    {
        // Choose the road prefab to instantiate
        int prefabIndex = prefabOrder[currentPrefabIndex];
        GameObject road = Instantiate(roadPrefabs[prefabIndex]);

        // Set the parent to keep the hierarchy clean
        road.transform.SetParent(transform);

        // Position the road in the scene
        road.transform.position = new Vector3(0, 0, spawnZ);

        // Spawn the snow particle system above the road
        SpawnSnowAboveRoad(road);

        // Update spawnZ to place the next road further along the Z-axis
        spawnZ += roadLength;

        // Cycle through road prefabs
        currentPrefabIndex = (currentPrefabIndex + 1) % prefabOrder.Length;
    }

    private void SpawnSnowAboveRoad(GameObject road)
    {
        // Instantiate the snow particle system above the road
        Vector3 snowPosition = road.transform.position + new Vector3(0, 10, 0);  // Adjust Y for the snow to fall from above the road
        GameObject snow = Instantiate(snowPrefab, snowPosition, Quaternion.identity);

        // Optionally, set the snow particle system as a child of the road so it moves with it
        snow.transform.SetParent(road.transform);
    }
}
