using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float timeTravelPerTile = 10f;
    TerrainSpawner terrainSpawner;
    float currentCoord;
    int nextCoord = 0;
    float moveSpeed;
    float terrainSize;
    void Start()
    {
        terrainSpawner = FindObjectOfType<TerrainSpawner>();
        terrainSize = terrainSpawner.terrainSize;
        moveSpeed = terrainSpawner.terrainSize / timeTravelPerTile;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        currentCoord = posToCoord(transform.position); // start from -1 when xStart = 0
        //trigger (once) TerrainSpawner when just move in next tile (nextCoord)
        if (nextCoord - Mathf.Floor(currentCoord) == 0)
        {
            nextCoord++;
            terrainSpawner.SpawnOnCoord((int)Mathf.Floor(currentCoord));
        }
    }

    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }
}
