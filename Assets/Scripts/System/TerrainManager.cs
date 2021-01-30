using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    NORMAL, ARENA
}
public class TerrainManager : MonoBehaviour
{
    [SerializeField] public float terrainSize = 512;
    [SerializeField] List<GameObject> normalTiles;
    [SerializeField] List<GameObject> arenaTiles;
    int lastSpawnedTileIndex = -1; // this property is to make sure two adjacent tiles not similar
    int currentCoord = 0;
    // Start is called before the first frame update
    void Start()
    {
        // hide all tiles
        foreach (Transform child in transform)
        {
            //tiles.Add(child.gameObject);
            child.position = new Vector3(0, -50, 0);
        }
        SpawnOnCoord(0, TerrainType.NORMAL);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // coord: 0->256 1->256+512
    public void SpawnOnCoord(int coord, TerrainType type)
    {
        int randomIndex = 0;
        GameObject spawnTile = default;

        // TODO: refactor, choose the tile list based on TerrainType
        switch (type)
        {
            case TerrainType.NORMAL:
                randomIndex = Helper.RandomIntExcept(0, normalTiles.Count, lastSpawnedTileIndex);
                spawnTile = normalTiles[randomIndex];
                break;
            case TerrainType.ARENA:
                randomIndex = Helper.RandomIntExcept(0, arenaTiles.Count, lastSpawnedTileIndex);
                spawnTile = arenaTiles[randomIndex];
                break;
        }

        spawnTile.transform.position = new Vector3(terrainSize / 2 + coord * terrainSize, 0, -terrainSize / 2);
        lastSpawnedTileIndex = randomIndex;

        // print out objects on the spawned terrain
        // foreach (Transform child in normalTiles[randomIndex].transform)
        // {
        //     // to move childrent with terrain, don't mark it static
        //     print(child.name);
        //     print(child.transform.position);
        // }
    }
}
