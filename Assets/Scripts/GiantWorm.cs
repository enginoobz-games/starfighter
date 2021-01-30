using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWorm : MonoBehaviour
{
    enum AttackStates { attack1, attack2, attack3 };
    [SerializeField] Transform target;
    [SerializeField] Animator anim;
    [SerializeField] AttackStates currentAttackState = AttackStates.attack1;
    public float disappearDistance = 70f;
    public float appearDistance = 120f;
    [SerializeField] float appearY = 10f;
    [SerializeField] float appearZ = 20f; // -32 -> 32 (scale 17)


    string[] attackStates = new string[] { "attack1", "attack2", "attack3" };
    bool reappearTriggered = false;
    bool lookingAtTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < CameraRig.Instance.transform.position.x + disappearDistance && !reappearTriggered)
        {
            StartCoroutine(nameof(Reappear));
        }
    }

    private void FixedUpdate()
    {
        if (lookingAtTarget)
        {
            Vector3 lTargetDir = target.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 1);
        }
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
        Appear(new Vector3(CameraRig.Instance.transform.position.x + appearDistance, appearY, appearZ));
        reappearTriggered = false;
    }
}
