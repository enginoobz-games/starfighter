using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
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
        GameObject spawnPrefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject mine = Instantiate(spawnPrefab, spawnPos, spawnPrefab.transform.rotation);
        mine.transform.parent = transform;
        Destroy(mine, 15f);
    }
}
