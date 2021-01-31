using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviourSingleton<BossSpawner>
{
    [SerializeField] GiantWorm[] bosses;
    int lastSpawnedIndex = -1; // to make sure two adjacent bosses not similar

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        int randomIndex = Helper.RandomIntExcept(0, bosses.Length, lastSpawnedIndex);
        lastSpawnedIndex = randomIndex;

        GiantWorm spawnBoss = bosses[randomIndex];
        spawnBoss.gameObject.SetActive(true);
        spawnBoss.Appear(new Vector3(CameraRig.Instance.transform.position.x + spawnBoss.appearDistance, 10, -20));
    }
}
