using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // behavior: when player reachs this distance away from enemy, enemy is triggered and start following player for a duration then stops
    [SerializeField] Vector2 triggerDistanceRange = new Vector2(40f, 60f);
    [SerializeField] Vector2 followDurationRange = new Vector2(5f, 10f);
    [SerializeField] bool seriesExplosions = false; // for show case

    float triggerDistance; //= 50f;
    float followDuration; // = 10f;
    bool followingPlayer = false;
    bool followTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        Helper.SetupCollider(gameObject);
        triggerDistance = Random.Range(triggerDistanceRange.x, triggerDistanceRange.y);
        followDuration = Random.Range(followDurationRange.x, followDurationRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        // when player just comes closer to enemy at distance 40
        if (transform.position.x < CameraRig.Instance.transform.position.x + triggerDistance && !followTriggered)
        {
            followTriggered = true;
            followingPlayer = true;
            StartCoroutine(StopFollowing(followDuration));
        }

        if (followingPlayer)
        {
            transform.Translate(Vector3.back * Time.deltaTime * CameraRig.Instance.moveSpeed);
            // TODO: fire player
        }

        // destroy when player passes it
        if (transform.position.x < CameraRig.Instance.transform.position.x - 60)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StopFollowing(float time)
    {
        yield return new WaitForSeconds(time);
        followingPlayer = false;
    }

    public void OnDie(int cointOnDestroy) // string reference
    {
        CustomGameManager.Instance.UpdateCoint(cointOnDestroy);
        VfxManager.Instance.playExplosionFx(transform.position, 10f);
        Destroy(gameObject);

        if (seriesExplosions)
        {
            for (int i = 0; i < 10; i++)
            {
                VfxManager.Instance.playExplosionFx(new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1), transform.position.z + Random.Range(-1, 1)), 1f);
            }
        }
    }
}