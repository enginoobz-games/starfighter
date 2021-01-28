using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] explosionPrefabs;
    [SerializeField] ParticleSystem[] damagingPrefabs;

    // list storing all instances of all prefabs
    List<ParticleSystem> explosions = new List<ParticleSystem>();
    List<ParticleSystem> damagings = new List<ParticleSystem>();


    int poolSize = 3;

    // singleton pattern
    private static VfxManager _instance;
    public static VfxManager Instance { get { return _instance; } }
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
    // Start is called before the first frame update
    void Start()
    {
        // TODO: refactor similar functions
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

        foreach (ParticleSystem damagingPrefab in damagingPrefabs)
        {
            for (int i = 0; i < poolSize * 2; i++)
            {
                ParticleSystem fx = Instantiate(damagingPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                // set fx as child of this manager
                fx.transform.parent = transform;
                damagings.Add(fx);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: Implement scale of fx
    // https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/?_ga=2.111650306.720830859.1611209827-1791188190.1603910041&_gac=1.61708382.1607691575.Cj0KCQiAzsz-BRCCARIsANotFgPTC8836XZoHGo9MqAkf7KXBTPYDO_meogrMtsjwZ8IeA_nzP8JNzQaAlyWEALw_wcB
    public void playExplosionFx(Vector3 pos, float scale)
    {
        // TODO: choose index of last inactive element
        int randomIndex = Random.Range(0, explosions.Count);

        explosions[randomIndex].transform.position = pos;
        // ParticleSystem.MainModule mainModule = explosions[randomIndex].main;
        // mainModule.startSize = new ParticleSystem.MinMaxCurve { constantMin = mainModule.startSize.constantMin * scale, constantMax = mainModule.startSize.constantMax * scale };
        explosions[randomIndex].Play();
        // mainModule.startSize = new ParticleSystem.MinMaxCurve { constantMin = mainModule.startSize.constantMin / scale, constantMax = mainModule.startSize.constantMax / scale };

    }

    public void playDamagingFx(Vector3 pos, float scale)
    {
        int randomIndex = Random.Range(0, damagings.Count);

        damagings[randomIndex].transform.position = pos;
        damagings[randomIndex].Play();
    }
}
