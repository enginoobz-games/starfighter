using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class HealthSystem : MonoBehaviour
{
    [SerializeField] float immuneDuration = 2;
    [SerializeField] float maxHealth = 10;
    [SerializeField] TextMeshProUGUI label;
    float currentHealth;
    Collider theCollider;
    // Start is called before the first frame update
    void Start()
    {
        theCollider = GetComponent<MeshCollider>();
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
    }

    IEnumerator getDamaged()
    {
        currentHealth--;
        updateLabel();

        theCollider.enabled = false;
        yield return new WaitForSeconds(immuneDuration);
        theCollider.enabled = true;
    }

    private void updateLabel()
    {
        label.text = "Health " + currentHealth + "/" + maxHealth;
    }
}
