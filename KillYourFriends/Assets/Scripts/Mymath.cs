using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mymath
{
    public static float Slide(float current, float target, float percentLeft)
    {
        float p = 1 - Mathf.Pow(percentLeft, Time.deltaTime);
        return Mathf.Lerp(current, target, p);
    }
    public static Vector3 Slide(Vector3 current, Vector3 target, float percentLeft)
    {
        float p = 1 - Mathf.Pow(percentLeft, Time.deltaTime);
        return Vector3.Lerp(current, target, p);
    }
    public static Quaternion Slide(Quaternion current, Quaternion target, float percentLeft = 0.5f)
    {
        float p = 1 - Mathf.Pow(percentLeft, Time.deltaTime);
        return Quaternion.Lerp(current, target, p);
    }
}
