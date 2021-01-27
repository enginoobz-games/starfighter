using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] minePrefabs;
    [SerializeField] float yRange = 4f;
    [SerializeField] float zRange = 7f;
    [SerializeField] float spawnRate = 5f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Spawn()
    {
        Vector3 cameraPos = CameraRig.Instance.gameObject.transform.position;
        Vector3 spawnPos = cameraPos + new Vector3(300, Random.Range(-yRange, yRange), Random.Range(-zRange, zRange));
        GameObject mine = Instantiate(minePrefabs[Random.Range(0, minePrefabs.Length)], spawnPos, Quaternion.identity);
        mine.transform.parent = transform;
        Destroy(mine, 15f);
    }
}
