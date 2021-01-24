using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float timeTravelPerTile = 10f;
    [SerializeField] float accelerate = 0.018f;
    [SerializeField] float minTimeTravelPerTile = 3f;
    TerrainSpawner terrainSpawner;
    float currentCoord;
    int nextCoord = 0;
    float moveSpeed;
    float maxSpeed;
    float terrainSize;
    void Start()
    {
        terrainSpawner = FindObjectOfType<TerrainSpawner>();
        terrainSize = terrainSpawner.terrainSize;
        moveSpeed = terrainSize / timeTravelPerTile;
        maxSpeed = terrainSize / minTimeTravelPerTile;
        StartCoroutine(UpdateDistanceLabel(1f));
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        currentCoord = posToCoord(transform.position); // start from -1 when xStart = 0
        //trigger (once) TerrainSpawner when just move in next tile (nextCoord)
        if (nextCoord - Mathf.Floor(currentCoord) == 0)
        {
            nextCoord++;
            terrainSpawner.SpawnOnCoord((int)Mathf.Floor(currentCoord) + 1); // spawn 1 tile ahead
        }

        if (accelerate != 0f && moveSpeed <= maxSpeed)
        {
            moveSpeed += accelerate;
        }
    }

    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }

    IEnumerator UpdateDistanceLabel(float repeatRate)
    {
        while (true)
        {
            GameUI.Instance.UpdateDistance(Mathf.Round(transform.position.x));
            yield return new WaitForSeconds(repeatRate);
        }
    }
}
