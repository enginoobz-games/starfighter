using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // [SerializeField] GameObject explosionFX;
    // [SerializeField] GameObject damageFX;
    [SerializeField] int health = 5;
    [SerializeField] int coinOnDestroy;

    // ScoreLabel scoreLabel;
    VfxManager vfxManager;


    // Start is called before the first frame update
    void Start()
    {
        SetupCollider();
        vfxManager = FindObjectOfType<VfxManager>();
        // scoreLabel = FindObjectOfType<ScoreLabel>();
    }

    private void SetupCollider()
    {
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();

        // create new in case collider not included in pre-built asset
        if (meshCollider == null)
            meshCollider = gameObject.AddComponent<MeshCollider>();
        // disable trigger to interact with particles
        meshCollider.isTrigger = false;
        // enable convex to improve performance
        meshCollider.convex = true;
        // add mesh from Mesh Filter component to new Collinder component
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        vfxManager.playDamagingFx(transform.position, 0.05f);
        if (--health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // scoreLabel.UpdateOnEnemyDestroyed(scoreOnDestroy);
        // GameObject explosion = Instantiate(explosionFX, transform.position, Quaternion.identity);
        // Destroy(explosion, 1.5f);
        GameUI.Instance.UpdateCoint(coinOnDestroy);
        vfxManager.playExplosionFx(transform.position, 10f);
        Destroy(gameObject);
    }
}