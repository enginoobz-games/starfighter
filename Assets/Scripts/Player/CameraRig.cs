using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float timeTravelPerTile = 10f;
    public float timeTravelPerArena = 100f;
    public float accelerate = 0.3f;
    public float minTimeTravelPerTile = 3f;
    TerrainManager terrainManager;
    float currentCoord;
    int nextCoord = 0;
    bool enterArenaTriggered = false;
    bool isOnArena = false; // is fighting boss
    public float moveSpeed;
    float maxSpeed;
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

    public void SetTimeTravelPerTile(float duration)
    {
        timeTravelPerTile = duration;
        moveSpeed = terrainSize / timeTravelPerTile;
    }
    void Start()
    {
        terrainManager = FindObjectOfType<TerrainManager>();
        terrainSize = terrainManager.terrainSize;
        moveSpeed = terrainSize / timeTravelPerTile;
        maxSpeed = terrainSize / minTimeTravelPerTile;
    }

    void Update()
    {
        // update position & speed
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        if (accelerate != 0f && moveSpeed <= maxSpeed)
        {
            moveSpeed += accelerate;
        }

        currentCoord = posToCoord(transform.position); // start from -1 when xStart = 0
        //trigger (once) TerrainSpawner to spawn 1 tile ahead when just move in next tile (nextCoord)
        if (nextCoord == Mathf.Floor(currentCoord))
        {
            nextCoord++;
            TerrainType tileType = default;

            if (nextCoord < GameManager.Instance.bossCoord)
            {
                tileType = TerrainType.NORMAL;
            }
            // prepare arena tile ahead
            else if (nextCoord == GameManager.Instance.bossCoord)
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
            accelerate = 0f;
            moveSpeed = terrainSize / timeTravelPerArena;
            GameManager.Instance.OnBossAppear();
        }
    }

    public void AfterBossDefeat()
    {
        accelerate = 0.3f;
        moveSpeed = terrainSize / timeTravelPerTile;
        isOnArena = false;
    }

    // LOCAL HELPER
    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }
}
