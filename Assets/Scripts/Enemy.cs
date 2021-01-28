using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // [SerializeField] GameObject explosionFX;
    // [SerializeField] GameObject damageFX;
    [SerializeField] float health = 5f;
    [SerializeField] int cointOnDestroy;
    float timer = 10f; //first travel: same speed as player's, after timer start approaching player
    bool isMovingForwards = false;
    bool triggered = false;
    VfxManager vfxManager;


    // Start is called before the first frame update
    void Start()
    {
        SetupCollider();
        vfxManager = FindObjectOfType<VfxManager>();
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
        // when player just comes closer to enemy at distance 40
        if (transform.position.x < CameraRig.Instance.transform.position.x + 50 && !triggered)
        {
            triggered = true;
            isMovingForwards = true;
            StartCoroutine(StopMovingForwards(timer));
        }

        if (isMovingForwards)
        {
            transform.Translate(Vector3.back * Time.deltaTime * CameraRig.Instance.moveSpeed);
        }

        // destroy when player passes it
        if (transform.position.x < CameraRig.Instance.transform.position.x - 60)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StopMovingForwards(float time)
    {
        yield return new WaitForSeconds(time);
        isMovingForwards = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        GetDamaged(1f);
    }

    // TODO: destroy on terrain collision
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            Die();
        }
    }

    public void GetDamaged(float damage)
    {
        vfxManager.playDamagingFx(transform.position, 0.05f);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // scoreLabel.UpdateOnEnemyDestroyed(scoreOnDestroy);
        // GameObject explosion = Instantiate(explosionFX, transform.position, Quaternion.identity);
        // Destroy(explosion, 1.5f);
        GameUI.Instance.UpdateCoint(cointOnDestroy);
        vfxManager.playExplosionFx(transform.position, 10f);
        Destroy(gameObject);
    }
}