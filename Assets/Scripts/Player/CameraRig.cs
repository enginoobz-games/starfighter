﻿using System.Collections;
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
    bool enterArenaTrigger = false;
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
            // keep spawn arena tile ahead during boss fight
            // TODO: process after boss defeat
            else
            {
                terrainManager.SpawnOnCoord((int)Mathf.Floor(currentCoord) + 1, TerrainType.ARENA);

                // just enter, code inside excute only once
                if (!enterArenaTrigger)
                {
                    EnterBossArena();
                    enterArenaTrigger = true;
                }
            }
        }
    }

    private void EnterBossArena()
    {
        accelerate = 0f;
        moveSpeed = terrainSize / timeTravelPerArena;
        GameManager.Instance.TriggerBoss();
    }

    public void ExitBossArena()
    {
        accelerate = 0.3f;
        moveSpeed = terrainSize / timeTravelPerTile;
    }

    public float posToCoord(Vector3 pos)
    {
        return (pos.x - terrainSize / 2) / terrainSize;
    }
}
