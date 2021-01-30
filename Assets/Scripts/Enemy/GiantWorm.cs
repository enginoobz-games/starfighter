using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWorm : MonoBehaviour
{
    enum AttackStates { attack1, attack2, attack3 };
    [SerializeField] Transform target;
    [SerializeField] float lookAtSpeed = 0.5f;
    [SerializeField] Animator anim;
    [SerializeField] AttackStates currentAttackState = AttackStates.attack1;
    [SerializeField] AudioClip backgroundSound;
    public float disappearDistance = 70f;
    public float appearDistance = 120f;
    [SerializeField] float appearY = 10f;
    [SerializeField] Vector2 appearRangeZ = new Vector2(-25f, 25f);

    string[] attackStates = new string[] { "attack1", "attack2", "attack3" };
    bool lookingAtTarget = false;
    bool reappearTriggered = false;
    bool onDieTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onDieTriggered) return;
        if (transform.position.x < CameraRig.Instance.transform.position.x + disappearDistance && !reappearTriggered)
        {
            StartCoroutine(nameof(Reappear));
        }
    }

    private void FixedUpdate()
    {
        if (lookingAtTarget && !onDieTriggered)
        {
            Vector3 lTargetDir = target.position - transform.position;
            // lTargetDir.x = -50;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * lookAtSpeed);
        }
    }

    // TODO: stop colliding with the player
    public void OnDie(int cointOnDestroy) // string reference
    {
        if (onDieTriggered) return;

        GameManager.Instance.UpdateCoint(cointOnDestroy);

        for (int i = 0; i < 12; i++)
        {
            VfxManager.Instance.playExplosionFx(new Vector3(transform.position.x + Random.Range(-2, 2), Random.Range(40f, 70f), transform.position.z + Random.Range(-10, 10)), 1f);
        }
        anim.SetTrigger("death");
        onDieTriggered = true;
        StartCoroutine(nameof(Disable));
    }


    public void Appear(Vector3 pos)
    {
        transform.position = pos;
        lookingAtTarget = true;
        anim.SetTrigger("appear");
        // TODO: instead of delay, detect when "appear" state finishes then trigger
        StartCoroutine(Attack(2));
    }


    IEnumerator Attack(int delay)
    {
        yield return new WaitForSeconds(delay);
        // string attackState = attackStates[Random.Range(0, attackStates.Length)];
        string attackState = System.Enum.GetName(typeof(AttackStates), currentAttackState);
        // look at target instantly before attacking
        transform.LookAt(target);
        lookingAtTarget = false;
        anim.SetTrigger(attackState);
    }

    IEnumerator Reappear()
    {
        reappearTriggered = true;
        anim.SetTrigger("disappear");
        yield return new WaitForSeconds(3);
        Appear(new Vector3(CameraRig.Instance.transform.position.x + appearDistance, appearY, Random.Range(appearRangeZ.x, appearRangeZ.y)));
        reappearTriggered = false;
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(5f);
        onDieTriggered = false;
        gameObject.SetActive(false);
        GameManager.Instance.AfterBossDefeat();
    }
}
