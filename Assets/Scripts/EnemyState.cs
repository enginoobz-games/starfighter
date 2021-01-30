using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // TODO: destroy on terrain collision
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            print("enemy collides with terrain");
            // Die();
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        GetDamaged(1f);
    }

    public void GetDamaged(float damage)
    {
        VfxManager.Instance.playDamagingFx(transform.position, 0.05f);
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
