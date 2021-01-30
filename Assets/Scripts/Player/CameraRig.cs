using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float timeTravelPerTile = 10f;
    public float timeTravelPerArena = 100f;
    public float normalAccelerate = 0.3f;
    public float arenaAccelerate = 0f;
    public float minTimeTravelPerTile = 3f;
    TerrainManager terrainManager;
    float currentCoord;
    int nextCoord = 0;
    int nextBossCoord;
    bool enterArenaTriggered = false;
    bool isOnArena = false; // is fighting boss
    public float moveSpeed;
    float maxSpeed;
    float currentAccelerate;
    float terrainSize;

    // singleton pattern
    private static CameraRig _instance;
    public static CameraRig Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        terrainManager = FindObjectOfType<TerrainManager>();
        terrainSize = terrainManager.terrainSize;
        moveSpeed = terrainSize / timeTravelPerTile;
        maxSpeed = terrainSize / minTimeTravelPerTile;
        nextBossCoord = GameManager.Instance.bossOccurrence;
        currentAccelerate = normalAccelerate;
    }

    void Update()
    {
        // update position & speed
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        if (currentAccelerate != 0f && moveSpeed <= maxSpeed)
        {
            moveSpeed += currentAccelerate;
        }

        currentCoord = posToCoord(transform.position); // start from -1 when xStart = 0
        //trigger (once) TerrainSpawner to spawn 1 tile ahead when just move in next tile (nextCoord)
        if (nextCoord == Mathf.Floor(currentCoord))
        {
            nextCoord++;
            TerrainType tileType = default;

            // print("nextCoord: " + nextCoord + " - nextBossCoord: " + nextBossCoord);
            if (nextCoord < nextBossCoord)
            {
                tileType = TerrainType.NORMAL;
            }
            // prepare arena tile ahead
            else if (nextCoord == nextBossCoord)
            {
                tileType = TerrainType.ARENA;
            }
            else
            {
                OnBossEncounter();
                // keep spawn arena tile ahead during boss fight
                tileType = isOnArena ? TerrainType.ARENA : TerrainType.NORMAL;
            }

            terrainManager.SpawnOnCoord((int)Mathf.Floor(currentCoord) + 1, tileType);
        }
    }

    private void OnBossEncounter()
    {
        // trigger enter arena event, code inside excute only once
        if (!enterArenaTriggered)
        {
            enterArenaTriggered = true;
            isOnArena = true;
            currentAccelerate = arenaAccelerate;
            moveSpeed = terrainSize / timeTravelPerArena;
            GameManager.Instance.OnBossAppear();
        }
    }

    public void AfterBossDefeat()
    {
        currentAccelerate = normalAccelerate;
        moveSpeed = terrainSize / timeTravelPerTile;
        isOnArena = false;
        // reset tile counter until next boss appear
        nextBossCoord = nextCoord + GameManager.Instance.bossOccurrence;
        enterArenaTriggered = false;
    }

    // LOCAL HELPER
    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }
}
