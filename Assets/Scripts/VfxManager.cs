using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] explosionPrefabs;

    // list storing all instances of all prefabs
    List<ParticleSystem> explosions = new List<ParticleSystem>();

    int poolSize = 3;
    // Start is called before the first frame update
    void Start()
    {
        foreach (ParticleSystem explosionPrefab in explosionPrefabs)
        {
            for (int i = 0; i < poolSize; i++)
            {
                ParticleSystem fx = Instantiate(explosionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                // set fx as child of this manager
                fx.transform.parent = transform;
                explosions.Add(fx);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void playExplosionFx(Vector3 pos)
    {
        // TODO: choose index of last inactive element
        int randomIndex = Random.Range(0, explosions.Count - 1);
        explosions[randomIndex].transform.position = pos;
        explosions[randomIndex].Play();
    }
}
