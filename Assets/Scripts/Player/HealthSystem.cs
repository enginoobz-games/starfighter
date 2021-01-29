using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FXV;

//TODO: CHeck and add Collider & Rigidbody in runtime
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class HealthSystem : MonoBehaviour
{
    [SerializeField] float immuneDuration = 2;
    [SerializeField] float maxHealth = 10;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] FXVShield shieldAura;
    [SerializeField] FXVShield damagedShieldAura; // TODO: set same mesh of ship model for this shield in runtime

    VfxManager vfxManager;

    float currentHealth;
    Collider theCollider;
    bool isImmune = false;
    float shieldTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        theCollider = GetComponent<MeshCollider>();
        vfxManager = FindObjectOfType<VfxManager>();
        currentHealth = maxHealth;
        UpdateLabel();
        // startup shield
        damagedShieldAura.SetShieldActive(false);
        StartCoroutine(GetShielded(3f));
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
                Destroy(other);
                float duration = other.GetComponent<Shield>().duration;
                // TODO: improve logic
                if (shieldTimer == 0) // if not being shield, set timer & start counting down
                    StartCoroutine(GetShielded(duration));
                else // if being shield, just reset the timer
                    shieldTimer = duration;
                break;
            case "Shield Aura":
                break;
            default: // TODO: for terrain
                if (!isImmune)
                {
                    ContactPoint contact = collision.contacts[0];
                    vfxManager.playExplosionFx(contact.point, 0.5f);
                    StartCoroutine(nameof(GetDamaged));
                }
                break;
        }
    }

    public IEnumerator GetShielded(float duration)
    {
        isImmune = true;
        shieldAura.shieldActivationSpeed = 1f;
        shieldAura.SetShieldActive(true);
        shieldTimer = duration;

        while (shieldTimer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            shieldTimer--;

            if (shieldTimer == 0)
            {
                isImmune = false;
                shieldAura.shieldActivationSpeed = 0.5f;
                shieldAura.SetShieldActive(false);
            }
        }
    }

    IEnumerator GetDamaged()
    {
        currentHealth--;
        UpdateLabel();
        isImmune = true;
        damagedShieldAura.shieldActivationSpeed = 3f;
        damagedShieldAura.SetShieldActive(true);

        yield return new WaitForSeconds(immuneDuration);
        isImmune = false;
        damagedShieldAura.shieldActivationSpeed = 1f;
        damagedShieldAura.SetShieldActive(false);
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
