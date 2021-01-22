﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class HealthSystem : MonoBehaviour
{
    [SerializeField] float immuneDuration = 2;
    [SerializeField] float maxHealth = 10;
    float currentHealth;
    Collider theCollider;
    // Start is called before the first frame update
    void Start()
    {
        theCollider = GetComponent<MeshCollider>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(nameof(getDamaged));
    }

    IEnumerator getDamaged()
    {
        currentHealth--;
        theCollider.enabled = false;
        yield return new WaitForSeconds(immuneDuration);
        theCollider.enabled = true;
    }
}
