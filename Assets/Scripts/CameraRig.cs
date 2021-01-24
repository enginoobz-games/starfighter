using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float timeTravelPerTile = 10f;
    TerrainSpawner terrainSpawner;
    float currentCoord;
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

        //TODO: Trigger (once) TerrainSpawner when just move in new tile
        // currentCoord = posToCoord(transform.position);
        // print(Mathf.Floor(currentCoord) - currentCoord);
    }

    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }
}
