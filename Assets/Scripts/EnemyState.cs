using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Collider))]
public class EnemyState : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] int cointOnDestroy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // note: this object with RigidBody will receive OnCollisionEnter events from children as well
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GetDamaged(1f, contact.point);

        // TODO: destroy non-boss enemy on terrain collision
        if (collision.gameObject.CompareTag("Terrain"))
        {
            print("enemy collides with terrain");
            // Die();
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        print("OnParticleCollision");
        GetDamaged(1f, transform.position);
    }

    public void GetDamaged(float damage, Vector3 pos)
    {
        print(health);
        VfxManager.Instance.playDamagingFx(pos, 0.05f);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.UpdateCoint(cointOnDestroy);
        VfxManager.Instance.playExplosionFx(transform.position, 10f);
        Destroy(gameObject);
    }
}
