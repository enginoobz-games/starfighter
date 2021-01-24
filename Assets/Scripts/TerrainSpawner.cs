using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    [SerializeField] public float terrainSize;
    [SerializeField] Terrain[] tiles;

    int currentCoord = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(TestSpawn), 10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // coord: 0->256 1->256+512
    public void SpawnOnCoord(int coord)
    {
        tiles[0].transform.position = new Vector3(terrainSize / 2 + coord * terrainSize, 0, -terrainSize / 2);
    }

    public void TestSpawn()
    {
        SpawnOnCoord(++currentCoord);
    }
}
