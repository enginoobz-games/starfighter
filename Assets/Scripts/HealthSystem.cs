using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//TODO: CHeck and add Collider & Rigidbody in runtime
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class HealthSystem : MonoBehaviour
{
    [SerializeField] float immuneDuration = 2;
    [SerializeField] float maxHealth = 10;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] GameObject damagingVfx;
    VfxManager vfxManager;

    float currentHealth;
    Collider theCollider;
    // Start is called before the first frame update
    void Start()
    {
        theCollider = GetComponent<MeshCollider>();
        vfxManager = FindObjectOfType<VfxManager>();
        currentHealth = maxHealth;
        updateLabel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(nameof(getDamaged));

        ContactPoint contact = collision.contacts[0];
        vfxManager.playExplosionFx(contact.point);

        GameObject other = collision.gameObject;
        switch (other.gameObject.tag)
        {
            case "Mine":
                Destroy(other.gameObject);
                vfxManager.playExplosionFx(other.transform.position);
                break;
        }
    }

    IEnumerator getDamaged()
    {
        currentHealth--;
        updateLabel();
        theCollider.enabled = false;
        damagingVfx.SetActive(true);

        yield return new WaitForSeconds(immuneDuration);
        theCollider.enabled = true;
        damagingVfx.SetActive(false);
    }

    private void updateLabel()
    {
        label.text = "Health " + currentHealth + "/" + maxHealth;
    }
}
