using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] int cointOnDestroy;

    // behavior: when player reachs this distance away from enemy, enemy is triggered and start following player for a duration then stops
    [SerializeField] Vector2 triggerDistanceRange = new Vector2(40f, 60f);
    [SerializeField] Vector2 followDurationRange = new Vector2(5f, 10f);

    float triggerDistance; //= 50f;
    float followDuration; // = 10f;
    bool followingPlayer = false;
    bool followTriggered = false;
    VfxManager vfxManager;


    // Start is called before the first frame update
    void Start()
    {
        SetupCollider();
        vfxManager = FindObjectOfType<VfxManager>();
        triggerDistance = Random.Range(triggerDistanceRange.x, triggerDistanceRange.y);
        followDuration = Random.Range(followDurationRange.x, followDurationRange.y);
    }

    // HELPER
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
        if (transform.position.x < CameraRig.Instance.transform.position.x + triggerDistance && !followTriggered)
        {
            followTriggered = true;
            followingPlayer = true;
            StartCoroutine(StopFollowing(followDuration));
        }

        if (followingPlayer)
        {
            transform.Translate(Vector3.back * Time.deltaTime * CameraRig.Instance.moveSpeed);
            // TODO: fire player
        }

        // destroy when player passes it
        if (transform.position.x < CameraRig.Instance.transform.position.x - 60)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StopFollowing(float time)
    {
        yield return new WaitForSeconds(time);
        followingPlayer = false;
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
        GameUI.Instance.UpdateCoint(cointOnDestroy);
        vfxManager.playExplosionFx(transform.position, 10f);
        Destroy(gameObject);
    }
}