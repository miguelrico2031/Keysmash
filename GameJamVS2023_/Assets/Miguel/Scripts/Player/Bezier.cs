using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    public static Vector2 GetBezier(float time, float period, Vector2 start, Vector2 middle, Vector2 target)
    {
        float t = time / period;
        var ab = Vector2.Lerp(start,middle,time);
        var bc = Vector2.Lerp(middle,target,time);
        return Vector2.Lerp(ab,bc,time);
    }
}
