﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(nameof(getDamaged));

        switch (other.gameObject.tag)
        {
            case "Mine":
                Destroy(other.gameObject);
                // if (vfxManager)
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
