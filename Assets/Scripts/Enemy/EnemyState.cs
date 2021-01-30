using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Collider))]
public class EnemyState : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] int cointOnDestroy;

    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
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
        VfxManager.Instance.playDamagingFx(pos, 0.05f);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SendMessage("OnDie", cointOnDestroy);
        }
    }

    // if this enemy is disable for reuse, reset it current health
    private void OnDisable() {
        currentHealth = health;
    }
}
