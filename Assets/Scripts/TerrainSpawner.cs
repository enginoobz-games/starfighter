using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    [SerializeField] public float terrainSize = 512;
    List<GameObject> tiles = new List<GameObject>();

    int lastSpawnedTileIndex = -1; // to make sure two adjacent tiles not similar
    int currentCoord = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            tiles.Add(child.gameObject);
            // hide
            child.position = new Vector3(0, -50, 0);
        }
        SpawnOnCoord(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // coord: 0->256 1->256+512
    public void SpawnOnCoord(int coord)
    {
        int randomIndex = RandomIntExcept(0, tiles.Count, lastSpawnedTileIndex);
        tiles[randomIndex].transform.position = new Vector3(terrainSize / 2 + coord * terrainSize, 0, -terrainSize / 2);
        lastSpawnedTileIndex = randomIndex;
    }

    //HELPER
    public int RandomIntExcept(int min, int max, int except) // [min,max)
    {
        int result = Random.Range(min, max - 1);
        if (result >= except) result += 1;
        return result;
    }
}
