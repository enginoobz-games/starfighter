using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float timeTravelPerTile = 10f;
    [SerializeField] float accelerate = 0.018f;
    [SerializeField] float minTimeTravelPerTile = 3f;
    TerrainManager terrainManager;
    float currentCoord;
    int nextCoord = 0;
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
        if (nextCoord - Mathf.Floor(currentCoord) == 0)
        {
            nextCoord++;
            if (nextCoord < GameManager.Instance.bossCoord)
            {
                terrainManager.SpawnOnCoord((int)Mathf.Floor(currentCoord) + 1, TerrainType.NORMAL);
            }
            // prepare arena tile ahead
            else if (nextCoord == GameManager.Instance.bossCoord)
            {
                terrainManager.SpawnOnCoord((int)Mathf.Floor(currentCoord) + 1, TerrainType.ARENA); 
            }
            // keep spawn arena tile and setup params when fighting boss
            // TODO: trigger setup params only once when entering arena
            // TODO: process after boss defeat
            else
            {
                terrainManager.SpawnOnCoord((int)Mathf.Floor(currentCoord) + 1, TerrainType.ARENA); 
                EnterBossArena();
            }
        }
    }

    private void EnterBossArena()
    {
        accelerate = 0f;
        moveSpeed = 20f;
    }

    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }
}
