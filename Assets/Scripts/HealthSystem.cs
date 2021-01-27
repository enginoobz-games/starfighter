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
    bool isImmune = false;
    // Start is called before the first frame update
    void Start()
    {
        theCollider = GetComponent<MeshCollider>();
        vfxManager = FindObjectOfType<VfxManager>();
        currentHealth = maxHealth;
        UpdateLabel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "Mine":
            case "Enemy":
                Destroy(other);
                vfxManager.playExplosionFx(other.transform.position, 1f);
                if (!isImmune) StartCoroutine(nameof(GetDamaged));
                break;
            case "Heart":
                Heal(other.GetComponent<Heart>().healingAmount);
                Destroy(other);
                break;
            case "Shield":
                StartCoroutine(GetShielded(other.GetComponent<Shield>().duration));
                Destroy(other);
                break;
            default: // TODO: for terrain
                ContactPoint contact = collision.contacts[0];
                vfxManager.playExplosionFx(contact.point, 0.5f);
                if (!isImmune) StartCoroutine(nameof(GetDamaged));
                break;
        }
    }

    IEnumerator GetShielded(float duration)
    {
        isImmune = true;
        print(isImmune);

        yield return new WaitForSeconds(duration);
        isImmune = false;
    }

    IEnumerator GetDamaged()
    {
        currentHealth--;
        UpdateLabel();
        isImmune = true;
        damagingVfx.SetActive(true);

        yield return new WaitForSeconds(immuneDuration);
        isImmune = false;
        damagingVfx.SetActive(false);
    }

    private void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        label.text = "Health\n" + currentHealth + "/" + maxHealth;
    }
}
