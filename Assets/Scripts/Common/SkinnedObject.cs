using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedObject : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    [Tooltip("Warning: updating collider with skinned mesh is slow")]
    [SerializeField] float updateColliderRate = 0.5f;
    MeshCollider meshCollider;
    // Start is called before the first frame update
    void Start()
    {
        //meshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        transform.localScale = new Vector3(1 / parent.transform.localScale.x, 1 / parent.transform.localScale.y, 1 / parent.transform.localScale.z);

        InvokeRepeating(nameof(UpdateCollider), 3f, updateColliderRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = colliderMesh;
    }
}
