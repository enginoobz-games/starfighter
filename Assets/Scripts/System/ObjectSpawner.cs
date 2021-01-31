using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] float xRange = 100f;
    [SerializeField] float yRange = 4f;
    [SerializeField] float zRange = 7f;
    [SerializeField] float spawnRate = 5f;
    [SerializeField] bool autoDispose = true;
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
        Vector3 spawnPos = cameraPos + new Vector3(xRange, Random.Range(-yRange, yRange), Random.Range(-zRange, zRange));
        GameObject spawnPrefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject @object = Instantiate(spawnPrefab, spawnPos, spawnPrefab.transform.rotation);
        @object.transform.parent = transform;

        if (autoDispose)
            Destroy(@object, 30f);
    }
}
