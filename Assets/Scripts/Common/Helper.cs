using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    /* MATH */
    public static int RandomIntExcept(int min, int max, int except) // [min,max)
    {
        int result = Random.Range(min, max - 1);
        if (result >= except) result += 1;
        return result;
    }
    public static float WrapAngle(float angle) // to get the raw value as in inspector (include negative value, not 0-360)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    public static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    /* GAME OBJECT */
    public static void SetupCollider(GameObject gameObject)
    {
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();

        // create new in case collider not included in pre-built asset
        if (meshCollider == null)
            meshCollider = gameObject.AddComponent<MeshCollider>();
        // disable trigger to interact with particles
        meshCollider.isTrigger = false;
        // enable convex to improve performance
        meshCollider.convex = true;
        // add mesh from Mesh Filter component to new Collinder component
        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        if (mesh != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

}