using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileShooter : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] float shootRate = 1f;
    [SerializeField] float speed = 4;
    [SerializeField] float bulletDestroyDelay = 3f;
    float shootRateTimeStamp = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && Time.time > shootRateTimeStamp)
        {
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * speed * 1000);
            Destroy(bullet, bulletDestroyDelay);
            shootRateTimeStamp = Time.time + shootRate;
        }
    }
}
