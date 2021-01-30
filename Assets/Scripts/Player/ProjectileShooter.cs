using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileShooter : MonoBehaviour
{
    public bool use2D;
    public bool cameraShake;
    public GameObject firePoint;
    public GameObject cameras;

    public List<GameObject> VFXs = new List<GameObject>();

    // to configure direction of bullets, angleOffset.y = - Camera.tranform.rotation.y
    Vector3 rotateOffset = new Vector3(-10, 90, 0);
    private int count = 0;
    private float timeToFire = 0f;
    private GameObject effectToSpawn;
    private List<Camera> camerasList = new List<Camera>();
    private Camera singleCamera;

    void Start()
    {
        // InvokeRepeating(nameof(Next), 5f, 5f);

        if (cameras.transform.childCount > 0)
        {
            for (int i = 0; i < cameras.transform.childCount; i++)
            {
                camerasList.Add(cameras.transform.GetChild(i).gameObject.GetComponent<Camera>());
            }
            if (camerasList.Count == 0)
            {
                Debug.Log("Please assign one or more Cameras in inspector");
            }
        }
        else
        {
            singleCamera = cameras.GetComponent<Camera>();
            if (singleCamera != null)
                camerasList.Add(singleCamera);
            else
                Debug.Log("Please assign one or more Cameras in inspector");
        }

        if (VFXs.Count > 0)
            effectToSpawn = VFXs[0];
        else
            Debug.Log("Please assign one or more VFXs in inspector");

    }

    void Update()
    {
        UpdateRotateOffset();
        if (Input.GetKey(KeyCode.Space) && Time.time >= timeToFire || Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1f / effectToSpawn.GetComponent<ProjectileMoveScript>().fireRate;
            SpawnVFX();
        }

        if (Input.GetKeyDown(KeyCode.E))
            Next();
        if (Input.GetKeyDown(KeyCode.Q))
            Previous();
        // if (Input.GetKeyDown(KeyCode.C))
        //     SwitchCamera();
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        //     CameraShake();
        // if (Input.GetKeyDown(KeyCode.X))
        //     ZoomIn();
        // if (Input.GetKeyDown(KeyCode.Z))
        //     ZoomOut();
    }

    private void UpdateRotateOffset()
    {
        // TODO: parameterize
        // yPlayer: -30 -> 30
        // yOffset: 60  -> 120
        // xPlayer: -15 -> 15
        // xOffset: -25 -> 0
        float yRotatePlayer = Helper.WrapAngle(transform.parent.transform.localRotation.eulerAngles.y);
        rotateOffset.y = Helper.Remap(yRotatePlayer, -30, 30, 70, 110);
        float xRotatePlayer = Helper.WrapAngle(transform.parent.transform.localRotation.eulerAngles.x);
        rotateOffset.x = Helper.Remap(xRotatePlayer, -15, 15, -20, 0);
    }

    public void SpawnVFX()
    {
        GameObject vfx;

        var cameraShakeScript = cameras.GetComponent<CameraShakeSimpleScript>();

        if (cameraShake && cameraShakeScript != null)
            cameraShakeScript.ShakeCamera();

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity * Quaternion.Euler(rotateOffset.x, rotateOffset.y, rotateOffset.z));
        }
        else
            vfx = Instantiate(effectToSpawn);

        Destroy(vfx, 10f);

        var ps = vfx.GetComponent<ParticleSystem>();

        if (vfx.transform.childCount > 0)
        {
            ps = vfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        }
    }

    public void Next()
    {
        count++;

        if (count > VFXs.Count)
            count = 0;

        for (int i = 0; i < VFXs.Count; i++)
        {
            if (count == i) effectToSpawn = VFXs[i];
        }
    }

    public void Previous()
    {
        count--;

        if (count < 0)
            count = VFXs.Count;

        for (int i = 0; i < VFXs.Count; i++)
        {
            if (count == i) effectToSpawn = VFXs[i];
        }
    }

    public void CameraShake()
    {
        cameraShake = !cameraShake;
    }

    public void ZoomIn()
    {
        if (camerasList.Count > 0)
        {
            if (!camerasList[0].orthographic)
            {
                if (camerasList[0].fieldOfView < 101)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].fieldOfView += 5;
                    }
                }
            }
            else
            {
                if (camerasList[0].orthographicSize < 10)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].orthographicSize += 0.5f;
                    }
                }
            }
        }
    }

    public void ZoomOut()
    {
        if (camerasList.Count > 0)
        {
            if (!camerasList[0].orthographic)
            {
                if (camerasList[0].fieldOfView > 20)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].fieldOfView -= 5;
                    }
                }
            }
            else
            {
                if (camerasList[0].orthographicSize > 4)
                {
                    for (int i = 0; i < camerasList.Count; i++)
                    {
                        camerasList[i].orthographicSize -= 0.5f;
                    }
                }
            }
        }
    }

    public void SwitchCamera()
    {
        if (camerasList.Count > 0)
        {
            for (int i = 0; i < camerasList.Count; i++)
            {
                if (camerasList[i].gameObject.activeSelf)
                {
                    camerasList[i].gameObject.SetActive(false);
                    if ((i + 1) == camerasList.Count)
                    {
                        camerasList[0].gameObject.SetActive(true);
                        break;
                    }
                    else
                    {
                        camerasList[i + 1].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
